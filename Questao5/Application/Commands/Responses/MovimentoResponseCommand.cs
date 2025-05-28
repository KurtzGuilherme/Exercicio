namespace Questao5.Application.Commands.Responses;

public class MovimentoResponseCommand
{
    public string IdMovimento { get; }

    public MovimentoResponseCommand(string idMovimento)
    {
        IdMovimento = idMovimento;
    }
}
