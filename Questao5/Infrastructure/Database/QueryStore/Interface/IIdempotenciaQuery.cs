using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Interface;

public interface IIdempotenciaQuery
{
    Task<Idempotencia?> GetIdempotenciaByChave(string chave);
}
