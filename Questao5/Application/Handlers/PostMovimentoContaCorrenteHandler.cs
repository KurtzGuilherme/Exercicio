using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Extensions;
using Questao5.Infrastructure.Database.CommandStore.Interface;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using System.Text.Json;

namespace Questao5.Application.Handlers;

public class PostMovimentoContaCorrenteHandler : IRequestHandler<MovimentoRequestCommand, MovimentoResponseCommand>
{
    private readonly IContaCorrenteQuery _contaCorrenteQuery;
    private readonly IMovimentoCommand _movimentoCommand;
    private readonly IIdempotenciaCommand _idempotenciaCommand;
    private readonly IIdempotenciaQuery _idempotenciaQuery;

    public PostMovimentoContaCorrenteHandler(
        IContaCorrenteQuery contaCorrenteQuery,
        IMovimentoCommand movimentoCommand,
        IIdempotenciaCommand idempotenciaCommand,
        IIdempotenciaQuery idempotenciaQuery)
    {
        _contaCorrenteQuery = contaCorrenteQuery;
        _movimentoCommand = movimentoCommand;
        _idempotenciaCommand = idempotenciaCommand;
        _idempotenciaQuery = idempotenciaQuery;
    }
    public async Task<MovimentoResponseCommand> Handle(MovimentoRequestCommand request, CancellationToken cancellationToken)
    {
        ValidacaoRequest(request);

        if (!string.IsNullOrWhiteSpace(request.ChaveIdempotencia))
        {
            var idempotencia = await _idempotenciaQuery.GetIdempotenciaByChave(request.ChaveIdempotencia);
            if (idempotencia != null)
                return JsonSerializer.Deserialize<MovimentoResponseCommand>(idempotencia.Resultado);
        }

        var contaCorrente = await _contaCorrenteQuery.GetContaCorrentePorNumeroContaAsync(request.NumeroConta);

        ValidacaoMovimento(contaCorrente, request);

        var movimento = new Movimento(request.TipoMovimento, contaCorrente.IdContaCorrente, request.Valor);

        await _movimentoCommand.PostMovimentoContaCorrente(movimento);

        var response = new MovimentoResponseCommand(movimento.IdMovimento);

        await _idempotenciaCommand.Registrar(movimento.IdMovimento, JsonSerializer.Serialize(request), JsonSerializer.Serialize(response));

        return response;
    }

    private void ValidacaoRequest(MovimentoRequestCommand request)
    {
        if (request == null)
            throw new ArgumentNullOrEmptyException(nameof(request));

        if (request.NumeroConta <= 0)
            throw new ArgumentNullOrEmptyException(nameof(request));
    }

    private void ValidacaoMovimento(ContaCorrente? contaCorrente, MovimentoRequestCommand request)
    {
        if (contaCorrente == null)
            throw new NotificationException("Apenas contas correntes cadastradas podem receber movimentação.", "INVALID_ACCOUNT");

        if (contaCorrente.Ativo == TipoSituacao.Inativo)
            throw new NotificationException("Apenas contas correntes ativas podem receber movimentação.", "INACTIVE_ACCOUNT");

        if (request.Valor <= 0)
            throw new NotificationException("Apenas valores positivos podem ser recebidos.", "INVALID_VALUE");

        if (request.TipoMovimento != ((char)TipoMovimento.Credito).ToString() &&
            request.TipoMovimento != ((char)TipoMovimento.Debito).ToString())
            throw new NotificationException("Apenas os tipos “débito” ou “crédito” podem ser aceitos.", "INVALID_TYPE");
    }

}
