using PersonRegistry.Application.DTOs;
using PersonRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> ObterTodasAsync(int? skip = null, int? take = null);
        Task<Pessoa?> ObterPorCodigoAsync(int codigo);
        Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf);
        Task<Pessoa> AdicionarAsync(RequisicaoPessoaDto dto);
        Task<Pessoa?> AtualizarAsync(int codigo, RequisicaoPessoaDto dto);
        Task<bool> ExcluirAsync(int codigo);
    }
}
