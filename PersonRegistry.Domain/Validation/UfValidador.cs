namespace PersonRegistry.Domain.Validation
{
    public static class UfValidador
    {
        public static readonly IReadOnlySet<string> Validas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
            "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
            "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        public static bool EhValida(string? uf) => !string.IsNullOrWhiteSpace(uf) && Validas.Contains(uf);
    }
}
