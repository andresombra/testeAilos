using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IQueryStore
    {
        Task<ContaCorrente> ObterContaCorrenteAsync(string idContaCorrente);
        Task<decimal> ObterSaldoAsync(string idContaCorrente);
        Task<Idempotencia> ObterIdempotenciaAsync(string chaveIdempotencia);
    }
}
