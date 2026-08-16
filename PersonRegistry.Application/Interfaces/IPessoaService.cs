using PersonRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> ObterTodasAsync();
        Task<Pessoa?> ObterPorCodigoAsync(int codigo);
        Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf);
        Task<Pessoa> AdicionarAsync(Pessoa pessoa);
        Task<Pessoa?> AtualizarAsync(int codigo, Pessoa pessoa);
        Task<bool> ExcluirAsync(int codigo);
    }
}
