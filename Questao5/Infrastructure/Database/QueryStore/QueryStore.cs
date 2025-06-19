using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Models;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class QueryStore : IQueryStore
    {
        private readonly string _connectionString;

        public QueryStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ContaCorrente> ObterContaCorrenteAsync(string idContaCorrente)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                SELECT idcontacorrente as IdContaCorrente, 
                       numero as Numero, 
                       nome as Nome, 
                       ativo as Ativo
                FROM contacorrente 
                WHERE idcontacorrente = @IdContaCorrente";

            var conta = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
                sql, 
                new { IdContaCorrente = idContaCorrente }
                );

            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { IdContaCorrente = idContaCorrente });
        }

        public async Task<decimal> ObterSaldoAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                SELECT COALESCE(
                    SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END) - 
                    SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 
                    0
                ) as Saldo
                FROM movimento 
                WHERE idcontacorrente = @IdContaCorrente";

            var resultado = await connection.QueryFirstOrDefaultAsync<decimal?>(sql, new { IdContaCorrente = idContaCorrente });
            return resultado ?? 0;
        }

        public async Task<Idempotencia> ObterIdempotenciaAsync(string chaveIdempotencia)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                SELECT chave_idempotencia as ChaveIdempotencia,
                       requisicao as Requisicao,
                       resultado as Resultado
                FROM idempotencia 
                WHERE chave_idempotencia = @ChaveIdempotencia";

            return await connection.QueryFirstOrDefaultAsync<Idempotencia>(sql, new { ChaveIdempotencia = chaveIdempotencia });
        }
    }
}
