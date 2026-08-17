using PersonRegistry.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<string?> AutenticarAsync(RequisicaoLoginDto loginDto);
    }
}
