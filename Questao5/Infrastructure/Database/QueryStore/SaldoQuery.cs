using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.QueryStore.Interface;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class SaldoQuery : ISaldoQuery
{
    private readonly DatabaseConfig databaseConfig;

    public SaldoQuery(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public async Task<decimal> GetValorMovimentoByIdContaCorrenteAndTipoAsync(string idContaCorrente, TipoMovimento tipo)
    {
        var param = new
        {
            IdContaCorrente = idContaCorrente,
            TipoMovimento = ((char)tipo).ToString()
        };

        using var connection = new SqliteConnection(databaseConfig.Name);
        var result = await connection.QueryFirstOrDefaultAsync<decimal>(QuerysStatements.GetMovimentoByIdContaCorrenteAndTipoMovimento, param);

        return result;
    }
}
