using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class ContaCorrenteQuery : IContaCorrenteQuery
{
    private readonly DatabaseConfig databaseConfig;

    public ContaCorrenteQuery(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task<ContaCorrente?> GetContaCorrentePorNumeroContaAsync(int numeroConta)
    {
        var param = new { numero = numeroConta };

        using var connection = new SqliteConnection(databaseConfig.Name);
        var result = await connection.QueryFirstOrDefaultAsync<ContaCorrente?>(QuerysStatements.GetContaCorrenteByNumero, param);

        return result;
    }
}
