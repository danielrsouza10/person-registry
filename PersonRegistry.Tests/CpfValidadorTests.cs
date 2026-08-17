using PersonRegistry.Domain.Validation;
using Xunit;

namespace PersonRegistry.Tests
{
    public class CpfValidadorTests
    {
        [Theory]
        [InlineData("52998224725")]
        [InlineData("529.982.247-25")]
        public void EhValido_ComCpfValido_DeveRetornarTrue(string cpf)
        {
            Assert.True(CpfValidador.EhValido(cpf));
        }

        [Theory]
        [InlineData("11111111111")]
        [InlineData("12345678900")]
        [InlineData("123")]
        [InlineData("")]
        [InlineData(null)]
        public void EhValido_ComCpfInvalido_DeveRetornarFalse(string? cpf)
        {
            Assert.False(CpfValidador.EhValido(cpf));
        }
    }
}
