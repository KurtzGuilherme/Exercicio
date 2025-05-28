namespace Questao5.Infrastructure.Sqlite;

internal static class QuerysStatements
{
    internal const string GetContaCorrenteByNumero = @"
        SELECT 
            *
         FROM 
            contacorrente
         WHERE 
            numero = @numero";

    internal const string GetMovimentoByIdContaCorrenteAndTipoMovimento = @"
        SELECT 
          COALESCE(SUM(valor), 0) as valor
        FROM 
            movimento
        WHERE 
            idcontacorrente = @IdContaCorrente AND 
            tipomovimento = @TipoMovimento";

    internal const string PostMovimentoContaCorrente = @"
        INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
        VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

    internal const string RegistrarIdempotencia = @"
        INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
        VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

    internal const string GetIdempotenciaByChave = @"
        SELECT 
            chave_idempotencia as idempotenciaChave, 
            requisicao, 
            resultado 
        FROM 
            idempotencia 
        WHERE 
            chave_idempotencia = @IdempotenciaChave";
}