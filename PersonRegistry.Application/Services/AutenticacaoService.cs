using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Application.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PersonRegistry.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AdminSettings _adminSettings;

        public AutenticacaoService(IOptions<JwtSettings> jwtSettings, IOptions<AdminSettings> adminSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _adminSettings = adminSettings.Value;
        }

        public Task<string?> AutenticarAsync(RequisicaoLoginDto loginDto)
        {
            if (!string.Equals(loginDto.Username, _adminSettings.Username, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult<string?>(null);
            }

            if (string.IsNullOrWhiteSpace(_adminSettings.PasswordHash) ||
                !BCrypt.Net.BCrypt.Verify(loginDto.Password, _adminSettings.PasswordHash))
            {
                return Task.FromResult<string?>(null);
            }

            return Task.FromResult<string?>(GerarTokenJwt(loginDto.Username));
        }

        private string GerarTokenJwt(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jwtSettings.ObterChaveSimetrica();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Administrator")
            }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
