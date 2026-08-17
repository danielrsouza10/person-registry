namespace PersonRegistry.Domain.Validation
{
    public static class CpfValidador
    {
        public static string Normalizar(string cpf) => new(cpf.Where(char.IsDigit).ToArray());

        public static bool EhValido(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return false;
            }

            var digitos = Normalizar(cpf);

            if (digitos.Length != 11)
            {
                return false;
            }

            if (digitos.Distinct().Count() == 1)
            {
                return false;
            }

            var numeros = digitos.Select(c => c - '0').ToArray();

            var primeiroDigito = CalcularDigito(numeros, 10);
            if (primeiroDigito != numeros[9])
            {
                return false;
            }

            var segundoDigito = CalcularDigito(numeros, 11);
            return segundoDigito == numeros[10];
        }

        private static int CalcularDigito(int[] numeros, int pesoInicial)
        {
            var soma = 0;
            for (var i = 0; i < pesoInicial - 1; i++)
            {
                soma += numeros[i] * (pesoInicial - i);
            }

            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}
