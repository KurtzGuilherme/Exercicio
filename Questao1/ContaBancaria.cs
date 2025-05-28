using System;

namespace Questao1
{
    public sealed class ContaBancaria
    {
        public int Numero { get; }
        public string Titular { get; private set; }
        public double DepositoInicial { get; private set; }
        public double Saldo { get; private set; }

        private const double txSaque = 3.50;

        public ContaBancaria(
            int numero,
            string titular,
            double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            DepositoInicial = depositoInicial;

            Deposito(depositoInicial);
        }

        public ContaBancaria(
            int numero,
            string titular)
            : this(numero, titular, 0)
        {

        }

        public void Deposito(double valor)
        {
            if (valor < 0)
                throw new ArgumentException("O valor para deposito deve ser um valor positivo.");

            DepositoInicial = valor;

            AtualizarSaldo(valor);
        }

        public void Saque(double valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor para saque deve ser um valor positivo e maior que zero.");

            var valorSaque = (valor + txSaque) * -1;

            AtualizarSaldo(valorSaque);
        }

        private void AtualizarSaldo(double valor)
            => Saldo = Saldo + valor;

        //public string ToDadosBancarios()
        //{
        //    var dadosConta = $"Conta: {Numero}, Titular: {Titular}, Saldo: {Saldo.ToString("C2")} ";

        //    return dadosConta;
        //}

        public override string ToString()
            => $"Conta: {Numero}, Titular: {Titular}, Saldo: {Saldo.ToString("C2")} ";

    }
}
