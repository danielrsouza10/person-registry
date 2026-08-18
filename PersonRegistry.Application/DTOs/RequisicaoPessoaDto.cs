using PersonRegistry.Application.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PersonRegistry.Application.DTOs
{
    public class RequisicaoPessoaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;

        [JsonConverter(typeof(DataBrasileiraJsonConverter))]
        public DateTime? DataNascimento { get; set; }
    }
}
