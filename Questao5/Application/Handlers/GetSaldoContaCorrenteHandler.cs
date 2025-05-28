using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Extensions;
using Questao5.Infrastructure.Database.QueryStore.Interface;

namespace Questao5.Application.Handlers;

public class GetSaldoContaCorrenteHandler : IRequestHandler<SaldoRequestQuery, SaldoResponseQuery>
{

    private readonly IContaCorrenteQuery _contaCorrenteQuery;
    private readonly ISaldoQuery _saldoQuery;

    public GetSaldoContaCorrenteHandler(IContaCorrenteQuery contaCorrenteQuery, ISaldoQuery saldoQuery)
    {
        _contaCorrenteQuery = contaCorrenteQuery;
        _saldoQuery = saldoQuery;
    }

    public async Task<SaldoResponseQuery> Handle(SaldoRequestQuery request, CancellationToken cancellationToken)
    {
        ValidacaoRequest(request);

        var contaCorrente = await _contaCorrenteQuery.GetContaCorrentePorNumeroContaAsync(request.NumeroConta);

        ValidacaoContaCorrente(contaCorrente);

        var valorCredito = await _saldoQuery.GetValorMovimentoByIdContaCorrenteAndTipoAsync(
            contaCorrente.IdContaCorrente,
            TipoMovimento.Credito);

        var valorDebito = await _saldoQuery.GetValorMovimentoByIdContaCorrenteAndTipoAsync(
            contaCorrente.IdContaCorrente,
            TipoMovimento.Debito);

        var saldoAtual = 0m;


        saldoAtual = valorCredito - valorDebito;

        var SaldoContaCorrenteResponse = new SaldoResponseQuery(contaCorrente.Numero, contaCorrente.Nome, saldoAtual);

        return SaldoContaCorrenteResponse;
    }

    private static void ValidacaoRequest(SaldoRequestQuery request)
    {
        if (request == null)
            throw new ArgumentNullOrEmptyException(nameof(request));

        if (request.NumeroConta <= 0)
            throw new ArgumentNullOrEmptyException(nameof(request));
    }

    private static void ValidacaoContaCorrente(Domain.Entities.ContaCorrente? contaCorrente)
    {
        if (contaCorrente == null)
            throw new NotificationException("Apenas contas correntes cadastradas podem consultar o saldo.", "INVALID_ACCOUNT");

        if (contaCorrente.Ativo == TipoSituacao.Inativo)
            throw new NotificationException("Apenas contas correntes ativas podem consultar o saldo.", "INACTIVE_ACCOUNT");
    }

   
}
