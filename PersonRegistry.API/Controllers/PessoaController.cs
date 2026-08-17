using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;

namespace PersonRegistry.API.Controllers
{
    [Authorize]
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
            if (skip is < 0)
            {
                ModelState.AddModelError(nameof(skip), "O valor de skip não pode ser negativo.");
            }

            if (take is < 0)
            {
                ModelState.AddModelError(nameof(take), "O valor de take não pode ser negativo.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var pessoas = await _service.ObterTodasAsync(skip, take);
            return Ok(pessoas);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObterPorCodigo([FromRoute] int codigo)
        {
            var pessoa = await _service.ObterPorCodigoAsync(codigo);
            if (pessoa == null)
            {
                return Problem(
                    detail: $"Pessoa não encontrada com o código {codigo}.",
                    statusCode: StatusCodes.Status404NotFound);
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
        public async Task<IActionResult> Criar([FromBody] RequisicaoPessoaDto dto)
        {
            var pessoaSalva = await _service.AdicionarAsync(dto);
            return CreatedAtAction(nameof(ObterPorCodigo), new { codigo = pessoaSalva.Codigo }, pessoaSalva);
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Atualizar([FromRoute] int codigo, [FromBody] RequisicaoPessoaDto dto)
        {
            var pessoaAtualizada = await _service.AtualizarAsync(codigo, dto);

            if (pessoaAtualizada == null)
            {
                return Problem(
                    detail: "Pessoa não encontrada para atualização.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(pessoaAtualizada);
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Excluir([FromRoute] int codigo)
        {
            var excluido = await _service.ExcluirAsync(codigo);

            if (!excluido)
            {
                return Problem(
                    detail: "Pessoa não encontrada para exclusão.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            return NoContent();
        }
    }
}
