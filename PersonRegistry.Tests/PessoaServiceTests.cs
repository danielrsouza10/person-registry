using FluentValidation;
using Moq;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Services;
using PersonRegistry.Application.Validators;
using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using Xunit;

namespace PersonRegistry.Tests
{
    public class PessoaServiceTests
    {
        private const string CpfValido = "52998224725";

        private static RequisicaoPessoaDto CriarDtoValido() => new()
        {
            Nome = "Fulano de Tal",
            Cpf = CpfValido,
            Uf = "GO",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        private static PessoaService CriarService(Mock<IPessoaRepository> repositorioMock)
            => new(repositorioMock.Object, new PessoaValidator());

        [Fact]
        public async Task AdicionarAsync_ComDtoValido_DeveAdicionarPessoa()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExisteCpfAsync(CpfValido, null)).ReturnsAsync(false);
            repositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Pessoa>()))
                .ReturnsAsync((Pessoa p) => { p.Codigo = 1; return p; });

            var service = CriarService(repositorioMock);
            var dto = CriarDtoValido();

            var resultado = await service.AdicionarAsync(dto);

            Assert.Equal(1, resultado.Codigo);
            Assert.Equal(dto.Nome, resultado.Nome);
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Once);
        }

        [Fact]
        public async Task AdicionarAsync_ComCpfInvalido_DeveLancarValidationException()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            var service = CriarService(repositorioMock);
            var dto = CriarDtoValido();
            dto.Cpf = "11111111111";

            await Assert.ThrowsAsync<ValidationException>(() => service.AdicionarAsync(dto));
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
        }

        [Fact]
        public async Task AdicionarAsync_ComCpfJaCadastrado_DeveLancarValidationException()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExisteCpfAsync(CpfValido, null)).ReturnsAsync(true);

            var service = CriarService(repositorioMock);
            var dto = CriarDtoValido();

            await Assert.ThrowsAsync<ValidationException>(() => service.AdicionarAsync(dto));
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarAsync_ComCpfDoPropriaPessoa_NaoDeveConsiderarDuplicado()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExisteCpfAsync(CpfValido, 5)).ReturnsAsync(false);
            repositorioMock.Setup(r => r.AtualizarAsync(It.IsAny<Pessoa>()))
                .ReturnsAsync((Pessoa p) => p);

            var service = CriarService(repositorioMock);
            var dto = CriarDtoValido();

            var resultado = await service.AtualizarAsync(5, dto);

            Assert.NotNull(resultado);
            repositorioMock.Verify(r => r.ExisteCpfAsync(CpfValido, 5), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_QuandoPessoaNaoExiste_DeveRetornarNull()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExisteCpfAsync(CpfValido, 99)).ReturnsAsync(false);
            repositorioMock.Setup(r => r.AtualizarAsync(It.IsAny<Pessoa>())).ReturnsAsync((Pessoa?)null);

            var service = CriarService(repositorioMock);

            var resultado = await service.AtualizarAsync(99, CriarDtoValido());

            Assert.Null(resultado);
        }

        [Fact]
        public async Task AdicionarAsync_ComCpfFormatado_DeveArmazenarSomenteDigitos()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExisteCpfAsync(CpfValido, null)).ReturnsAsync(false);
            repositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Pessoa>()))
                .ReturnsAsync((Pessoa p) => { p.Codigo = 1; return p; });

            var service = CriarService(repositorioMock);
            var dto = CriarDtoValido();
            dto.Cpf = "529.982.247-25";

            var resultado = await service.AdicionarAsync(dto);

            Assert.Equal(CpfValido, resultado.Cpf);
            repositorioMock.Verify(r => r.AdicionarAsync(It.Is<Pessoa>(p => p.Cpf == CpfValido)), Times.Once);
        }

        [Fact]
        public async Task ObterTodasAsync_DeveRetornarMetadadosDePaginacao()
        {
            var pessoas = new List<Pessoa>
            {
                new() { Codigo = 1, Nome = "Pessoa 1", Cpf = CpfValido, Uf = "GO", DataNascimento = DateTime.Now },
                new() { Codigo = 2, Nome = "Pessoa 2", Cpf = CpfValido, Uf = "SP", DataNascimento = DateTime.Now }
            };

            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ObterTodasAsync(0, 2)).ReturnsAsync(pessoas);
            repositorioMock.Setup(r => r.ContarAsync()).ReturnsAsync(10);

            var service = CriarService(repositorioMock);

            var resultado = await service.ObterTodasAsync(0, 2);

            Assert.Equal(10, resultado.Total);
            Assert.Equal(0, resultado.Skip);
            Assert.Equal(2, resultado.Take);
            Assert.Equal(2, resultado.Itens.Count());
        }

        [Fact]
        public async Task ExcluirAsync_DevePassarCodigoParaRepositorio()
        {
            var repositorioMock = new Mock<IPessoaRepository>();
            repositorioMock.Setup(r => r.ExcluirAsync(3)).ReturnsAsync(true);

            var service = CriarService(repositorioMock);

            var resultado = await service.ExcluirAsync(3);

            Assert.True(resultado);
            repositorioMock.Verify(r => r.ExcluirAsync(3), Times.Once);
        }
    }
}
