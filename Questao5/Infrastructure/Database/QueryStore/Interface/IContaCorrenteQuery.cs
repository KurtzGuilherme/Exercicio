using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Interface;

public interface IContaCorrenteQuery
{
    Task<ContaCorrente?> GetContaCorrentePorNumeroContaAsync(int numeroConta);
}
