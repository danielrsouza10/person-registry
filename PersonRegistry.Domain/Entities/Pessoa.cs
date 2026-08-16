using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Domain.Entities
{
    public class Pessoa
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
    }
}
