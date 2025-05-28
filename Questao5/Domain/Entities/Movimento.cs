namespace Questao5.Domain.Entities;

public sealed class Movimento
{
    public string IdMovimento { get; private set; }
    public string IdContaCorrente { get; private set; }
    public string DataMovimento { get; private set; }
    public string TipoMovimento { get; private set; }
    public decimal Valor { get; private set; }

    public Movimento(
        string idMovimento,
        string idContaCorrente,
        string dataMovimento,
        string tipoMovimento,
        decimal valor)
    {
        IdMovimento = idMovimento.ToUpper();
        IdContaCorrente = idContaCorrente.ToUpper();
        DataMovimento = dataMovimento;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }

    public Movimento(string tipoMovimento, string idContaCorrente, decimal valor)
        :this(Guid.NewGuid().ToString(), idContaCorrente, DateTime.Now.ToString("dd-MM-yyyy"), tipoMovimento, valor)
    {       
    }

    public Movimento()
    {             
    }
}
