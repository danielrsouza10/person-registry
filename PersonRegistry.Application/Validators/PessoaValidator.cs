using FluentValidation;
using PersonRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Validators
{
    public class PessoaValidator : AbstractValidator<Pessoa>
    {
        public PessoaValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(p => p.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.Now).WithMessage("A data de nascimento não pode ser no futuro.");

            RuleFor(p => p.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Length(11, 14).WithMessage("O CPF deve ter entre 11 e 14 caracteres.");

            RuleFor(p => p.Uf)
                .NotEmpty().WithMessage("A UF é obrigatória.")
                .Length(2).WithMessage("A UF deve conter exatamente 2 caracteres (Ex: GO, SP).");
        }
    }
}
