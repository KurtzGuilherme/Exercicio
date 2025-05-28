using Bogus;
using Moq;
using Moq.AutoMock;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Extensions;
using Questao5.Infrastructure.Database.CommandStore.Interface;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using System.Text.Json;
using Xunit;

namespace Questao5.Test.Handles;

public class PostMovimentoContaCorrenteHandlerTest
{

    private readonly AutoMocker _autoMock;

    public PostMovimentoContaCorrenteHandlerTest()
    {
        _autoMock = new AutoMocker();
    }

    [Fact]  
    public async Task Handler_RequestNull_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var paramName = "request";
        MovimentoRequestCommand fakeRequest = null;
        
        //Act
        var ex = await Assert.ThrowsAsync<ArgumentNullOrEmptyException>(() 
            => handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(paramName, ex.ParamName);
            Assert.NotNull(ex.Message);
        });
    }

    [Fact]
    public async Task Handler_NumeroContaZero_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
            .RuleFor(x => x.ChaveIdempotencia, string.Empty)
            .RuleFor(x => x.NumeroConta, 0)
            .RuleFor(x => x.TipoMovimento, "C")
            .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
            .Generate();
        var paramName = "request";

        //Act
        var ex = await Assert.ThrowsAsync<ArgumentNullOrEmptyException>(() =>
            handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(paramName, ex.ParamName);
            Assert.NotNull(ex.Message);
        });
    }

    [Fact]
    public async Task Handler_ContaCorrenteNulo_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, string.Empty)
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "C")
             .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
             .Generate();
        var codeMensagem = "INVALID_ACCOUNT";

        _autoMock.GetMock<IContaCorrenteQuery>()
            .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
            .ReturnsAsync((ContaCorrente)null);

        //Act
        var ex = await Assert.ThrowsAsync<NotificationException>(() =>
            handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(codeMensagem, ex.Code);
            Assert.NotNull(ex.Message);

            _autoMock.GetMock<IContaCorrenteQuery>()
               .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handler_ContaCorrenteInativa_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, string.Empty)
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "C")
             .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
             .Generate();
        var contaCorrenteInativaFake = new Faker<ContaCorrente>()
            .RuleFor(x => x.IdContaCorrente, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Numero, fakeRequest.NumeroConta)
            .RuleFor(x => x.Nome, f => f.Person.FullName)
            .RuleFor(x => x.Ativo, f => TipoSituacao.Inativo)
            .Generate();
        var codeMensagem = "INACTIVE_ACCOUNT";

        _autoMock.GetMock<IContaCorrenteQuery>()
            .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
            .ReturnsAsync(contaCorrenteInativaFake);

        //Act
        var ex = await Assert.ThrowsAsync<NotificationException>(() =>
            handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(codeMensagem, ex.Code);
            Assert.NotNull(ex.Message);

            _autoMock.GetMock<IContaCorrenteQuery>()
               .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handler_TipoMovimentoInvalido_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, string.Empty)
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "B")
             .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
             .Generate();
        var contaCorrenteInativaFake = new Faker<ContaCorrente>()
            .RuleFor(x => x.IdContaCorrente, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Numero, fakeRequest.NumeroConta)
            .RuleFor(x => x.Nome, f => f.Person.FullName)
            .RuleFor(x => x.Ativo, f => TipoSituacao.Ativo)
            .Generate();
        var codeMensagem = "INVALID_TYPE";

        _autoMock.GetMock<IContaCorrenteQuery>()
            .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
            .ReturnsAsync(contaCorrenteInativaFake);

        //Act
        var ex = await Assert.ThrowsAsync<NotificationException>(() =>
            handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(codeMensagem, ex.Code);
            Assert.NotNull(ex.Message);

            _autoMock.GetMock<IContaCorrenteQuery>()
               .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handler_valorZero_DeveRetornarExcecao()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, string.Empty)
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "C")
             .RuleFor(x => x.Valor, 0)
             .Generate();
        var contaCorrenteInativaFake = new Faker<ContaCorrente>()
            .RuleFor(x => x.IdContaCorrente, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Numero, fakeRequest.NumeroConta)
            .RuleFor(x => x.Nome, f => f.Person.FullName)
            .RuleFor(x => x.Ativo, f => TipoSituacao.Ativo)
            .Generate();
        var codeMensagem = "INVALID_VALUE";

        _autoMock.GetMock<IContaCorrenteQuery>()
            .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
            .ReturnsAsync(contaCorrenteInativaFake);

        //Act
        var ex = await Assert.ThrowsAsync<NotificationException>(() =>
            handler.Handle(fakeRequest, CancellationToken.None));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(codeMensagem, ex.Code);
            Assert.NotNull(ex.Message);

            _autoMock.GetMock<IContaCorrenteQuery>()
               .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handler_SemChaveIdempotencia_DeveRetornarMovimentoResponseCommand()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, string.Empty)
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "C")
             .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
             .Generate();
        var contaCorrenteInativaFake = new Faker<ContaCorrente>()
            .RuleFor(x => x.IdContaCorrente, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Numero, fakeRequest.NumeroConta)
            .RuleFor(x => x.Nome, f => f.Person.FullName)
            .RuleFor(x => x.Ativo, f => TipoSituacao.Ativo)
            .Generate();
        
        _autoMock.GetMock<IContaCorrenteQuery>()
            .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
            .ReturnsAsync(contaCorrenteInativaFake);

        _autoMock.GetMock<IMovimentoCommand>()
          .Setup(x => x.PostMovimentoContaCorrente(It.IsAny<Movimento>()));

        _autoMock.GetMock<IIdempotenciaCommand>()
          .Setup(x => x.Registrar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        //Act
        var result = await handler.Handle(fakeRequest, CancellationToken.None);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.NotNull(result.IdMovimento);

            _autoMock.GetMock<IContaCorrenteQuery>()
               .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()), Times.Once);
            _autoMock.GetMock<IMovimentoCommand>()
               .Verify(x => x.PostMovimentoContaCorrente(It.IsAny<Movimento>()), Times.Once);
            _autoMock.GetMock<IIdempotenciaCommand>()
               .Verify(x => x.Registrar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        });
    }

    [Fact]
    public async Task Handler_ComChaveIdempotencia_DeveRetornarMovimentoResponseCommand()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<MovimentoRequestCommand>()
             .RuleFor(x => x.ChaveIdempotencia, f => f.Random.Guid().ToString())
             .RuleFor(x => x.NumeroConta, f => f.Random.Int(1, 999))
             .RuleFor(x => x.TipoMovimento, "C")
             .RuleFor(x => x.Valor, f => Math.Round(f.Random.Decimal(1.00m, 1000.00m), 2))
             .Generate();

        var requisicao = JsonSerializer.Serialize(fakeRequest);
        var resultado = JsonSerializer.Serialize(new MovimentoResponseCommand(fakeRequest.ChaveIdempotencia));

        var idempotenciaFake = new Faker<Idempotencia>()
            .RuleFor(x => x.IdempotenciaChave, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Resultado, resultado)
            .RuleFor(x => x.Requisicao, requisicao)
            .Generate();

        _autoMock.GetMock<IIdempotenciaQuery>()
          .Setup(x => x.GetIdempotenciaByChave(It.IsAny<string>()))
          .ReturnsAsync(idempotenciaFake);

        //Act
        var result = await handler.Handle(fakeRequest, CancellationToken.None);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.NotNull(result.IdMovimento);

            _autoMock.GetMock<IIdempotenciaQuery>()
               .Verify(x => x.GetIdempotenciaByChave(It.IsAny<string>()), Times.Once);
        });
    }

    public PostMovimentoContaCorrenteHandler Handler()
        => new PostMovimentoContaCorrenteHandler(
            _autoMock.GetMock<IContaCorrenteQuery>().Object,
            _autoMock.GetMock<IMovimentoCommand>().Object,
            _autoMock.GetMock<IIdempotenciaCommand>().Object,
            _autoMock.GetMock<IIdempotenciaQuery>().Object
        );

}
