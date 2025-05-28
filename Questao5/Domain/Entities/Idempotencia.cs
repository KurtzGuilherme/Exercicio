namespace Questao5.Domain.Entities;

public sealed class Idempotencia
{
    public string IdempotenciaChave { get; private set; }
    public string Requisicao { get; private set; }
    public string Resultado { get; private set; }

    public Idempotencia(
        string idempotenciaChave,
        string requisicao,
        string resultado)
    {
        IdempotenciaChave = idempotenciaChave.ToUpper();
        Requisicao = requisicao;
        Resultado = resultado;
    }

    public Idempotencia()
    {
            
    }
}
