using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class CommandStore : ICommandStore
    {
        private readonly string _connectionString;

        public CommandStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InserirMovimentoAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            await connection.ExecuteAsync(sql, movimento);
        }

        public async Task InserirIdempotenciaAsync(Idempotencia idempotencia)
        {
            using var connection = new SqliteConnection(_connectionString);

            const string sql = @"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

            await connection.ExecuteAsync(sql, idempotencia);
        }
    }
}
