using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class IdempotenciaQuery : IIdempotenciaQuery
{
    private readonly DatabaseConfig databaseConfig;
    public IdempotenciaQuery(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task<Idempotencia?> GetIdempotenciaByChave(string chave)
    {
        var param = new 
        {
            IdempotenciaChave = chave.ToUpper()
        };

        using var connection = new SqliteConnection(databaseConfig.Name);
        var result = await connection.QueryFirstOrDefaultAsync<Idempotencia?>(QuerysStatements.GetIdempotenciaByChave, param);

        return result;
    }
}
