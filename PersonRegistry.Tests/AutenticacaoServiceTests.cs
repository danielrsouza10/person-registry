using Microsoft.Extensions.Options;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Services;
using PersonRegistry.Application.Settings;
using Xunit;

namespace PersonRegistry.Tests
{
    public class AutenticacaoServiceTests
    {
        private static AutenticacaoService CriarService(string username, string senhaEmTexto)
        {
            var adminSettings = new AdminSettings
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(senhaEmTexto)
            };
            var jwtSettings = new JwtSettings
            {
                Secret = "chave-secreta-para-testes-unitarios-1234567890",
                ExpirationHours = 1
            };

            return new AutenticacaoService(Options.Create(jwtSettings), Options.Create(adminSettings));
        }

        [Fact]
        public void Autenticar_ComCredenciaisCorretas_DeveRetornarToken()
        {
            var service = CriarService("admin", "admin123");

            var token = service.Autenticar(new RequisicaoLoginDto { Username = "admin", Password = "admin123" });

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void Autenticar_ComSenhaIncorreta_DeveRetornarNull()
        {
            var service = CriarService("admin", "admin123");

            var token = service.Autenticar(new RequisicaoLoginDto { Username = "admin", Password = "senha-errada" });

            Assert.Null(token);
        }

        [Fact]
        public void Autenticar_ComUsuarioInexistente_DeveRetornarNull()
        {
            var service = CriarService("admin", "admin123");

            var token = service.Autenticar(new RequisicaoLoginDto { Username = "outro-usuario", Password = "admin123" });

            Assert.Null(token);
        }

        [Fact]
        public void Autenticar_ComUsernameEmCasoDiferente_DeveAutenticar()
        {
            var service = CriarService("admin", "admin123");

            var token = service.Autenticar(new RequisicaoLoginDto { Username = "ADMIN", Password = "admin123" });

            Assert.NotNull(token);
        }
    }
}
