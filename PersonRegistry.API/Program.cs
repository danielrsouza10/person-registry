using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PersonRegistry.API.ExceptionHandling;
using PersonRegistry.Application.DTOs;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Application.Services;
using PersonRegistry.Application.Settings;
using PersonRegistry.Application.Validators;
using PersonRegistry.Domain.Interfaces;
using PersonRegistry.Infrastructure.Repositories;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

builder.Services.AddSingleton<IPessoaRepository, PessoaRepository>();

builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<IValidator<RequisicaoPessoaDto>, PessoaValidator>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminSettings"));


var jwtSecret = builder.Configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("A chave JWT n�o foi configurada.");

var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();