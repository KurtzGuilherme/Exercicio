using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.CommandStore.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore;

public class IdempotenciaCommand : IIdempotenciaCommand
{
    private readonly DatabaseConfig databaseConfig;

    public IdempotenciaCommand(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task Registrar(string idempotenciaId, string requisicao, string resultado)
    {
        var param = new {

            ChaveIdempotencia = idempotenciaId.ToUpper(),
            Requisicao =  requisicao,
            Resultado = resultado
        };
        using var connection = new SqliteConnection(databaseConfig.Name);
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(QuerysStatements.RegistrarIdempotencia, param);

        await transaction.CommitAsync();
    }
}
