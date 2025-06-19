using MediatR;
using Questao5.Application.Commands.Responses;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand : IRequest<MovimentarContaResponse>
    {
        [Required]
        public string IdRequisicao { get; set; }

        [Required]
        public string IdContaCorrente { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required]
        [RegularExpression("^[CD]$", ErrorMessage = "Tipo deve ser C (Crédito) ou D (Débito)")]
        public string TipoMovimento { get; set; }
    }
}
