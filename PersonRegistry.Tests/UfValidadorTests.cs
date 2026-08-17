using PersonRegistry.Domain.Validation;
using Xunit;

namespace PersonRegistry.Tests
{
    public class UfValidadorTests
    {
        [Theory]
        [InlineData("GO")]
        [InlineData("SP")]
        [InlineData("go")]
        [InlineData("sp")]
        public void EhValida_ComUfExistente_DeveRetornarTrue(string uf)
        {
            Assert.True(UfValidador.EhValida(uf));
        }

        [Theory]
        [InlineData("XX")]
        [InlineData("ZZ")]
        [InlineData("G")]
        [InlineData("GOI")]
        [InlineData("")]
        [InlineData(null)]
        public void EhValida_ComUfInexistente_DeveRetornarFalse(string? uf)
        {
            Assert.False(UfValidador.EhValida(uf));
        }
    }
}
