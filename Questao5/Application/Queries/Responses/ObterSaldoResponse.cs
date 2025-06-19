namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoResponse
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; }
        public string DataHoraConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
        public bool Sucesso { get; set; }
        public string TipoErro { get; set; }
        public string Mensagem { get; set; }

        public static ObterSaldoResponse Success(int numero, string nome, decimal saldo)
        {
            return new ObterSaldoResponse
            {
                NumeroContaCorrente = numero,
                NomeTitular = nome,
                DataHoraConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                SaldoAtual = saldo,
                Sucesso = true
            };
        }

        public static ObterSaldoResponse Error(string tipoErro, string mensagem)
        {
            return new ObterSaldoResponse
            {
                Sucesso = false,
                TipoErro = tipoErro,
                Mensagem = mensagem
            };
        }
    }
}
