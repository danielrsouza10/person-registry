using FluentValidation;
using PersonRegistry.Application.DTOs;
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
        private readonly IValidator<RequisicaoPessoaDto> _validator;

        public PessoaService(IPessoaRepository repository, IValidator<RequisicaoPessoaDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<Pessoa>> ObterTodasAsync(int? skip = null, int? take = null)
            => await _repository.ObterTodasAsync(skip, take);

        public async Task<Pessoa?> ObterPorCodigoAsync(int codigo)
            => await _repository.ObterPorCodigoAsync(codigo);

        public async Task<IEnumerable<Pessoa>> ObterPorUfAsync(string uf)
            => await _repository.ObterPorUfAsync(uf);

        public async Task<Pessoa> AdicionarAsync(RequisicaoPessoaDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Uf = dto.Uf,
                DataNascimento = dto.DataNascimento
            };

            return await _repository.AdicionarAsync(pessoa);
        }

        public async Task<Pessoa?> AtualizarAsync(int codigo, RequisicaoPessoaDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            var pessoa = new Pessoa
            {
                Codigo = codigo,
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Uf = dto.Uf,
                DataNascimento = dto.DataNascimento
            };

            return await _repository.AtualizarAsync(pessoa);
        }

        public async Task<bool> ExcluirAsync(int codigo)
            => await _repository.ExcluirAsync(codigo);
    }
}
