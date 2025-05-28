using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests;

public class MovimentoRequestCommand : IRequest<MovimentoResponseCommand>
{
    public string? ChaveIdempotencia { get; set; }
    public int NumeroConta { get; set; }
    public string TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}
