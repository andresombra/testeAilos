using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        private const double TAXA_SAQUE = 3.50;

        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        // Construtor com depósito inicial
        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        // Construtor sem depósito inicial
        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

    }
}
