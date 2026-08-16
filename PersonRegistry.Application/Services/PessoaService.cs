using FluentValidation;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _repository;
        private readonly IValidator<Pessoa> _validator;

        public PessoaService(IPessoaRepository repository, IValidator<Pessoa> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<Pessoa>> ObterTodasAsync()
            => await _repository.ObterTodasAsync();

        public async Task<Pessoa?> ObterPorCodigoAsync(int codigo)
            => await _repository.ObterPorCodigoAsync(codigo);

        public async Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf)
            => await _repository.ObterPorUfAsync(uf);

        public async Task<Pessoa> AdicionarAsync(Pessoa pessoa)
        {
            await _validator.ValidateAndThrowAsync(pessoa);

            return await _repository.AdicionarAsync(pessoa);
        }

        public async Task<Pessoa?> AtualizarAsync(int codigo, Pessoa pessoa)
        {
            await _validator.ValidateAndThrowAsync(pessoa);

            pessoa.Codigo = codigo;

            return await _repository.AtualizarAsync(pessoa);
        }

        public async Task<bool> ExcluirAsync(int codigo)
            => await _repository.ExcluirAsync(codigo);
    }
}
