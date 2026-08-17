using Microsoft.AspNetCore.Mvc;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;

namespace PersonRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;
        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] RequisicaoLoginDto loginDto)
        {
            var token = _autenticacaoService.Autenticar(loginDto);

            if (token == null)
            {
                return Unauthorized(new { Mensagem = "Credenciais inválidas. Verifique usuário e senha." });
            }

            return Ok(new { Token = token, Tipo = "Bearer" });
        }
    }
}
