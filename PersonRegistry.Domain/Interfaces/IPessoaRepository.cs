using PersonRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> ObterTodasAsync(int? skip = null, int? take = null);
        Task<Pessoa?> ObterPorCodigoAsync(int codigo);
        Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf);
        Task<bool> ExisteCpfAsync(string cpf, int? codigoIgnorar = null);
        Task<Pessoa> AdicionarAsync(Pessoa pessoa);
        Task<Pessoa?> AtualizarAsync(Pessoa pessoa);
        Task<bool> ExcluirAsync(int codigo);
    }
}
