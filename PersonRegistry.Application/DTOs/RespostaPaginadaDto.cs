namespace PersonRegistry.Application.DTOs
{
    public class RespostaPaginadaDto<T>
    {
        public IEnumerable<T> Itens { get; set; } = Enumerable.Empty<T>();
        public int Total { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }
    }
}
