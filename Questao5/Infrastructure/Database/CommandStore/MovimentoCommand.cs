using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore;

public class MovimentoCommand : IMovimentoCommand
{
    private readonly DatabaseConfig databaseConfig;
    public MovimentoCommand(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task PostMovimentoContaCorrente(Movimento movimento)
    {
        var param = new
        {
            movimento.IdMovimento,
            movimento.IdContaCorrente,
            movimento.DataMovimento,
            movimento.TipoMovimento,
            movimento.Valor
        };

        using var connection = new SqliteConnection(databaseConfig.Name);
        await connection.OpenAsync();
        
        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(QuerysStatements.PostMovimentoContaCorrente, param);

        await transaction.CommitAsync();
    }
}
