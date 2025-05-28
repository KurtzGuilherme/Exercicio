namespace Questao5.Application.Queries.Responses;

public class SaldoResponseQuery
{
    public int NumeroConta { get; private set; }
    public string NomeCliente { get; private set; }
    public decimal SaldoAtual { get; private set; }
    public DateTime Data { get; private set; }

    public SaldoResponseQuery(
        int numeroConta,
        string nomeCliente,
        decimal saldoAtual,
        DateTime data)
    {
        NumeroConta = numeroConta;
        NomeCliente = nomeCliente;
        SaldoAtual = saldoAtual;
        Data = data;
    }

    public SaldoResponseQuery(
        int numeroConta,
        string nomeCliente,
        decimal saldoAtual)
        :this(numeroConta, nomeCliente, saldoAtual, DateTime.Now)   
    {
        
    }

}
