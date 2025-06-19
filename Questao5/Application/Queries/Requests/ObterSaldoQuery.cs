using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ObterSaldoQuery : IRequest<ObterSaldoResponse>
    {
        public string IdContaCorrente { get; set; }

        public ObterSaldoQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}
