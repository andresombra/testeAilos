using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
    {
        private readonly ICommandStore _commandStore;
        private readonly IQueryStore _queryStore;

        public MovimentarContaHandler(ICommandStore commandStore, IQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            // Verificar idempotência
            var idempotenciaExistente = await _queryStore.ObterIdempotenciaAsync(request.IdRequisicao);
            if (idempotenciaExistente != null)
            {
                return MovimentarContaResponse.Success(idempotenciaExistente.Resultado);
            }

            // Validar conta
            var conta = await _queryStore.ObterContaCorrenteAsync(request.IdContaCorrente);
            if (conta == null)
            {
                return MovimentarContaResponse.Error(TipoErro.INVALID_ACCOUNT, "Conta corrente não encontrada");
            }

            if (conta.Ativo != 1)
            {
                return MovimentarContaResponse.Error(TipoErro.INACTIVE_ACCOUNT, "Conta corrente está inativa");
            }

            // Validar valor
            if (request.Valor <= 0)
            {
                return MovimentarContaResponse.Error(TipoErro.INVALID_VALUE, "Apenas valores positivos são aceitos");
            }

            // Validar tipo de movimento
            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            {
                return MovimentarContaResponse.Error(TipoErro.INVALID_TYPE, "Apenas os tipos 'C' (Crédito) ou 'D' (Débito) são aceitos");
            }

            // Processar movimentação
            var idMovimento = Guid.NewGuid().ToString();
            await _commandStore.InserirMovimentoAsync(new Domain.Entities.Movimento
            {
                IdMovimento = idMovimento,
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            });

            // Salvar idempotência
            await _commandStore.InserirIdempotenciaAsync(new Domain.Entities.Idempotencia
            {
                ChaveIdempotencia = request.IdRequisicao,
                Requisicao = System.Text.Json.JsonSerializer.Serialize(request),
                Resultado = idMovimento
            });

            return MovimentarContaResponse.Success(idMovimento);
        }
    }
}
