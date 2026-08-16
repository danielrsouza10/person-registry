using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly List<Pessoa> _pessoas = new();
        private int idAtual = 0;
        
        public Task<IEnumerable<Pessoa>> ObterTodasAsync()
            => Task.FromResult(_pessoas.AsEnumerable());

        public Task<Pessoa?> ObterPorCodigoAsync(int codigo)
            => Task.FromResult(_pessoas.FirstOrDefault(p => p.Codigo == codigo));

        public Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf)
            => Task.FromResult(_pessoas.Where(p => p.Uf == uf));

        public Task<Pessoa> AdicionarAsync(Pessoa pessoa)
        {
            pessoa.Codigo = idAtual++;
            _pessoas.Add(pessoa);
            return Task.FromResult(pessoa);
        }

        public Task<Pessoa?> AtualizarAsync(Pessoa pessoa)
        {
            var existentePessoa = _pessoas.FirstOrDefault(p => p.Codigo == pessoa.Codigo);

            if (existentePessoa != null)
            {
                existentePessoa.Nome = pessoa.Nome;
                existentePessoa.Cpf = pessoa.Cpf;
                existentePessoa.Uf = pessoa.Uf;
                existentePessoa.DataNascimento = pessoa.DataNascimento;
            }

            return Task.FromResult<Pessoa?>(existentePessoa);
        }

        public Task<bool> ExcluirAsync(int codigo)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (pessoa != null)
            {
                _pessoas.Remove(pessoa);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
