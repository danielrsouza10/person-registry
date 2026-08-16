using FluentValidation;
using System.Text;
using PersonRegistry.Domain.Entities;
using PersonRegistry.Domain.Interfaces;
using PersonRegistry.Application.Interfaces;
using PersonRegistry.Application.Services;
using PersonRegistry.Application.Validators;
using PersonRegistry.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IPessoaRepository, PessoaRepository>();

builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<IValidator<Pessoa>, PessoaValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();