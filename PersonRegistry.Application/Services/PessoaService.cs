using FluentValidation;
using FluentValidation.Results;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using PersonRegistry.Domain.Validation;
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

        public async Task<RespostaPaginadaDto<RespostaPessoaDto>> ObterTodasAsync(int? skip = null, int? take = null)
        {
            var pessoas = await _repository.ObterTodasAsync(skip, take);
            var total = await _repository.ContarAsync();

            return new RespostaPaginadaDto<RespostaPessoaDto>
            {
                Itens = pessoas.Select(ParaRespostaDto),
                Total = total,
                Skip = skip ?? 0,
                Take = take
            };
        }

        public async Task<RespostaPessoaDto?> ObterPorCodigoAsync(int codigo)
        {
            var pessoa = await _repository.ObterPorCodigoAsync(codigo);
            return pessoa == null ? null : ParaRespostaDto(pessoa);
        }

        public async Task<IEnumerable<RespostaPessoaDto>> ObterPorUfAsync(string uf)
        {
            var pessoas = await _repository.ObterPorUfAsync(uf);
            return pessoas.Select(ParaRespostaDto);
        }

        public async Task<RespostaPessoaDto> AdicionarAsync(RequisicaoPessoaDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);
            var cpfNormalizado = CpfValidador.Normalizar(dto.Cpf);
            await GarantirCpfNaoDuplicadoAsync(cpfNormalizado);

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Cpf = cpfNormalizado,
                Uf = dto.Uf,
                DataNascimento = dto.DataNascimento!.Value
            };

            var pessoaSalva = await _repository.AdicionarAsync(pessoa);
            return ParaRespostaDto(pessoaSalva);
        }

        public async Task<RespostaPessoaDto?> AtualizarAsync(int codigo, RequisicaoPessoaDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);
            var cpfNormalizado = CpfValidador.Normalizar(dto.Cpf);
            await GarantirCpfNaoDuplicadoAsync(cpfNormalizado, codigo);

            var pessoa = new Pessoa
            {
                Codigo = codigo,
                Nome = dto.Nome,
                Cpf = cpfNormalizado,
                Uf = dto.Uf,
                DataNascimento = dto.DataNascimento!.Value
            };

            var pessoaAtualizada = await _repository.AtualizarAsync(pessoa);
            return pessoaAtualizada == null ? null : ParaRespostaDto(pessoaAtualizada);
        }

        public async Task<bool> ExcluirAsync(int codigo)
            => await _repository.ExcluirAsync(codigo);

        private async Task GarantirCpfNaoDuplicadoAsync(string cpf, int? codigoIgnorar = null)
        {
            if (await _repository.ExisteCpfAsync(cpf, codigoIgnorar))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(Pessoa.Cpf), "Já existe uma pessoa cadastrada com este CPF.")
                });
            }
        }

        private static RespostaPessoaDto ParaRespostaDto(Pessoa pessoa) => new()
        {
            Codigo = pessoa.Codigo,
            Nome = pessoa.Nome,
            Cpf = pessoa.Cpf,
            Uf = pessoa.Uf,
            DataNascimento = pessoa.DataNascimento
        };
    }
}
