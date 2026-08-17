using PersonRegistry.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<RespostaPaginadaDto<RespostaPessoaDto>> ObterTodasAsync(int? skip = null, int? take = null);
        Task<RespostaPessoaDto?> ObterPorCodigoAsync(int codigo);
        Task<IEnumerable<RespostaPessoaDto>> ObterPorUfAsync(string uf);
        Task<RespostaPessoaDto> AdicionarAsync(RequisicaoPessoaDto dto);
        Task<RespostaPessoaDto?> AtualizarAsync(int codigo, RequisicaoPessoaDto dto);
        Task<bool> ExcluirAsync(int codigo);
    }
}
