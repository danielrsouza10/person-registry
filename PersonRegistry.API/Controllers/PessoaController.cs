using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Application.Services;

namespace PersonRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _service;
        public PessoaController(IPessoaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas([FromQuery] int? skip, [FromQuery] int? take)
        {
            var pessoas = await _service.ObterTodasAsync(skip, take);
            return Ok(pessoas);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObterPorCodigo([FromRoute] int codigo)
        {
            var pessoa = await _service.ObterPorCodigoAsync(codigo);
            if (pessoa == null)
            {
                return NotFound(new { Mensagem = $"Pessoa não encontrada com o codigo {codigo}." });
            }
            return Ok(pessoa);
        }

        [HttpGet("uf/{uf}")]
        public async Task<IActionResult> ObterPorUf([FromRoute] string uf)
        {
            var pessoas = await _service.ObterPorUfAsync(uf);
            return Ok(pessoas);
        }

        [HttpPost]
        public async Task<IActionResult> Gravar([FromBody] RequisicaoPessoaDto dto)
        {
            try
            {
                var pessoaSalva = await _service.AdicionarAsync(dto);

                return CreatedAtAction(nameof(ObterPorCodigo), new { codigo = pessoaSalva.Codigo }, pessoaSalva);
            }
            catch (ValidationException ex)
            {
                var erros = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Mensagem = "Erro de validação", Erros = erros });
            }
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Atualizar([FromRoute] int codigo, [FromBody] RequisicaoPessoaDto dto)
        {
            try
            {
                var pessoaAtualizada = await _service.AtualizarAsync(codigo, dto);

                if (pessoaAtualizada == null)
                    return NotFound(new { Mensagem = "Pessoa não encontrada para atualização." });

                return Ok(pessoaAtualizada);
            }
            catch (ValidationException ex)
            {
                var erros = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { Mensagem = "Erro de validação", Erros = erros });
            }
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Excluir([FromRoute] int codigo)
        {
            var excluido = await _service.ExcluirAsync(codigo);

            if (!excluido)
                return NotFound(new { Mensagem = "Pessoa não encontrada para exclusão." });

            return NoContent();
        }
    }
}
