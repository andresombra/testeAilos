namespace Questao5.Application.Commands.Responses
{
    public class MovimentarContaResponse
    {
        public string IdMovimento { get; set; }
        public bool Sucesso { get; set; }
        public string TipoErro { get; set; }
        public string Mensagem { get; set; }

        public static MovimentarContaResponse Success(string idMovimento)
        {
            return new MovimentarContaResponse
            {
                IdMovimento = idMovimento,
                Sucesso = true
            };
        }

        public static MovimentarContaResponse Error(string tipoErro, string mensagem)
        {
            return new MovimentarContaResponse
            {
                Sucesso = false,
                TipoErro = tipoErro,
                Mensagem = mensagem
            };
        }
    }
}
