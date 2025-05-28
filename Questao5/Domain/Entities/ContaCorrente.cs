using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities;

public sealed class ContaCorrente
{
    public string IdContaCorrente { get; private set; }
    public int Numero { get; private set; }
    public string Nome{ get; private set; }
    public TipoSituacao Ativo { get; private set; }

    public ContaCorrente(
        string idcontacorrente,
        int numeto,
        string nome, 
        TipoSituacao ativo)
    {
       IdContaCorrente = idcontacorrente;
        Numero = numeto;
        Nome = nome;
        Ativo = ativo;
    }

    public ContaCorrente()
    {
    }
}
