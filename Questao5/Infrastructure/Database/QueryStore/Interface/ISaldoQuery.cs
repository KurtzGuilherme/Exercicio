using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.QueryStore.Interface;

public interface ISaldoQuery
{
    Task<decimal> GetValorMovimentoByIdContaCorrenteAndTipoAsync(string idContaCorrente, TipoMovimento tipo);
}
