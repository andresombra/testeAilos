using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoHandler : IRequestHandler<ObterSaldoQuery, ObterSaldoResponse>
    {
        private readonly IQueryStore _queryStore;

        public ObterSaldoHandler(IQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<ObterSaldoResponse> Handle(ObterSaldoQuery request, CancellationToken cancellationToken)
        {
            // Validar conta
            var conta = await _queryStore.ObterContaCorrenteAsync(request.IdContaCorrente);
            if (conta == null)
            {
                return ObterSaldoResponse.Error(TipoErro.INVALID_ACCOUNT, "Conta corrente não encontrada");
            }

            if (conta.Ativo != 1)
            {
                return ObterSaldoResponse.Error(TipoErro.INACTIVE_ACCOUNT, "Conta corrente está inativa");
            }

            // Calcular saldo
            var saldo = await _queryStore.ObterSaldoAsync(request.IdContaCorrente);

            return ObterSaldoResponse.Success(conta.Numero, conta.Nome, saldo);
        }
    }
}
