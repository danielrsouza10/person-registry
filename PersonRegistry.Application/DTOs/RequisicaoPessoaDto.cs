using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.DTOs
{
    public class RequisicaoPessoaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
    }
}
