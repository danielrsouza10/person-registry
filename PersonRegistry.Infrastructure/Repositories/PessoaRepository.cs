using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using System.Collections.Concurrent;

namespace PersonRegistry.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly ConcurrentDictionary<int, Pessoa> _pessoas = new();
        private int _idAtual;

        public Task<IEnumerable<Pessoa>> ObterTodasAsync(int? skip = null, int? take = null)
        {
            var query = _pessoas.Values.OrderBy(p => p.Codigo).AsEnumerable();

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return Task.FromResult(query);
        }

        public Task<Pessoa?> ObterPorCodigoAsync(int codigo)
            => Task.FromResult(_pessoas.TryGetValue(codigo, out var pessoa) ? pessoa : null);

        public Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf)
            => Task.FromResult(_pessoas.Values.Where(p => string.Equals(p.Uf, uf, StringComparison.OrdinalIgnoreCase)).AsEnumerable());

        public Task<bool> ExisteCpfAsync(string cpf, int? codigoIgnorar = null)
        {
            var existe = _pessoas.Values.Any(p =>
                p.Codigo != codigoIgnorar &&
                string.Equals(SomenteDigitos(p.Cpf), SomenteDigitos(cpf), StringComparison.Ordinal));

            return Task.FromResult(existe);
        }

        public Task<Pessoa> AdicionarAsync(Pessoa pessoa)
        {
            pessoa.Codigo = Interlocked.Increment(ref _idAtual);
            _pessoas[pessoa.Codigo] = pessoa;
            return Task.FromResult(pessoa);
        }

        public Task<Pessoa?> AtualizarAsync(Pessoa pessoa)
        {
            if (!_pessoas.TryGetValue(pessoa.Codigo, out var existentePessoa))
            {
                return Task.FromResult<Pessoa?>(null);
            }

            existentePessoa.Nome = pessoa.Nome;
            existentePessoa.Cpf = pessoa.Cpf;
            existentePessoa.Uf = pessoa.Uf;
            existentePessoa.DataNascimento = pessoa.DataNascimento;

            return Task.FromResult<Pessoa?>(existentePessoa);
        }

        public Task<bool> ExcluirAsync(int codigo)
            => Task.FromResult(_pessoas.TryRemove(codigo, out _));

        private static string SomenteDigitos(string valor)
            => new(valor.Where(char.IsDigit).ToArray());
    }
}
