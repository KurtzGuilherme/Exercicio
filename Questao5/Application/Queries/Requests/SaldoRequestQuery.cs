using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests;

public class SaldoRequestQuery : IRequest<SaldoResponseQuery>
{
    public int NumeroConta { get; }

    public SaldoRequestQuery(int numeroConta)
    {
        NumeroConta = numeroConta;
    }
}
