using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface ICommandStore
    {
        Task InserirMovimentoAsync(Movimento movimento);
        Task InserirIdempotenciaAsync(Idempotencia idempotencia);
    }
}
