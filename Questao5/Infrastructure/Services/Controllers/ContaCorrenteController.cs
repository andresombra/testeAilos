using Microsoft.AspNetCore.Mvc;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    /// <summary>
    /// Controller para operações de conta corrente
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza uma movimentação na conta corrente
        /// </summary>
        /// <param name="command">Dados da movimentação</param>
        /// <returns>ID do movimento criado</returns>
        /// <response code="200">Movimentação realizada com sucesso</response>
        /// <response code="400">Dados inválidos ou regra de negócio violada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("movimentacao")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Movimentacao([FromBody] MovimentarContaCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _mediator.Send(command);

                if (!response.Sucesso)
                {
                    return BadRequest(new
                    {
                        Tipo = response.TipoErro,
                        Mensagem = response.Mensagem
                    });
                }

                return Ok(new { IdMovimento = response.IdMovimento });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Erro interno do servidor", Detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Consulta o saldo atual da conta corrente
        /// </summary>
        /// <param name="idContaCorrente">ID da conta corrente</param>
        /// <returns>Dados do saldo da conta</returns>
        /// <response code="200">Consulta realizada com sucesso</response>
        /// <response code="400">Conta inválida ou inativa</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{idContaCorrente}/saldo")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9")
        {
            try
            {
                var query = new ObterSaldoQuery(idContaCorrente);
                var response = await _mediator.Send(query);

                if (!response.Sucesso)
                {
                    return BadRequest(new
                    {
                        Tipo = response.TipoErro,
                        Mensagem = response.Mensagem
                    });
                }

                return Ok(new
                {
                    NumeroContaCorrente = response.NumeroContaCorrente,
                    NomeTitular = response.NomeTitular,
                    DataHoraConsulta = response.DataHoraConsulta,
                    SaldoAtual = response.SaldoAtual
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Erro interno do servidor", Detalhes = ex.Message });
            }
        }
    }
}
