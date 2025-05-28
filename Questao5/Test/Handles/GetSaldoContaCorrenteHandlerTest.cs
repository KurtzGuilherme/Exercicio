using Bogus;
using Moq;
using Moq.AutoMock;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Extensions;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using Xunit;

namespace Questao5.Test.Handles;

public class GetSaldoContaCorrenteHandlerTest
{
    private readonly AutoMocker _autoMock;

    public GetSaldoContaCorrenteHandlerTest()
    {
        _autoMock = new AutoMocker();
    }

    [Fact]
    public async Task Handler_RequestNulo_DeveLancarExcecao()
    {
        //Arrange
        var handler = Handler();
        var paramName = "request";
        SaldoRequestQuery fakeRequest = null;

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
        var fakeRequest = new Faker<SaldoRequestQuery>()
            .RuleFor(x => x.NumeroConta, 0)
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
        var fakeRequest = new Faker<SaldoRequestQuery>()
           .RuleFor(x => x.NumeroConta, f => f.Random.Number(1, 999))
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
        var fakeRequest = new Faker<SaldoRequestQuery>()
           .RuleFor(x => x.NumeroConta, f => f.Random.Number(1, 999))
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
    public async Task Handler_ContaCorrenteAtiva_DeveRetornarSaldoResponseQuery()
    {
        //Arrange
        var handler = Handler();
        var fakeRequest = new Faker<SaldoRequestQuery>()
            .RuleFor(x => x.NumeroConta, f => f.Random.Number(1, 999))
            .Generate();
        var contaCorrenteInativaFake = new Faker<ContaCorrente>()
            .RuleFor(x => x.IdContaCorrente, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Numero, fakeRequest.NumeroConta)
            .RuleFor(x => x.Nome, f => f.Person.FullName)
            .RuleFor(x => x.Ativo, f => TipoSituacao.Ativo)
            .Generate();
        var saldoCreditoFake = 1000m;
        var saldoDebitoFake = 100m;
        var saldoAtualFake = saldoCreditoFake - saldoDebitoFake;

        _autoMock.GetMock<IContaCorrenteQuery>()
       .Setup(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()))
       .ReturnsAsync(contaCorrenteInativaFake);

        _autoMock.GetMock<ISaldoQuery>()
           .Setup(x => x.GetValorMovimentoByIdContaCorrenteAndTipoAsync(It.IsAny<string>(), TipoMovimento.Credito))
           .ReturnsAsync(saldoCreditoFake);

        _autoMock.GetMock<ISaldoQuery>()
           .Setup(x => x.GetValorMovimentoByIdContaCorrenteAndTipoAsync(It.IsAny<string>(), TipoMovimento.Debito))
           .ReturnsAsync(saldoDebitoFake);

        //Act
        var result = await handler.Handle(fakeRequest, CancellationToken.None);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(saldoAtualFake, result.SaldoAtual);
            _autoMock.GetMock<IContaCorrenteQuery>()
                .Verify(x => x.GetContaCorrentePorNumeroContaAsync(It.IsAny<int>()),
                Times.Once);

            _autoMock.GetMock<ISaldoQuery>()
                .Verify(x => x.GetValorMovimentoByIdContaCorrenteAndTipoAsync(It.IsAny<string>(), It.IsAny<TipoMovimento>()),
                Times.AtMost(2));
        });
    }

    public GetSaldoContaCorrenteHandler Handler()
       => new GetSaldoContaCorrenteHandler(
          _autoMock.GetMock<IContaCorrenteQuery>().Object,
           _autoMock.GetMock<ISaldoQuery>().Object
       );
}
