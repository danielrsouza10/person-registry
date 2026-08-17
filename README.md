# PersonRegistry

API REST em ASP.NET Core (.NET 10) para cadastro de pessoas, com autenticação via JWT. Organizada em Clean Architecture.

## Estrutura da solução

```
PersonRegistry.API             Controllers, Program.cs (startup), appsettings
PersonRegistry.Application     DTOs, serviços, validadores (FluentValidation), settings
PersonRegistry.Domain          Entidades, interfaces de repositório, validações de domínio (CPF)
PersonRegistry.Infrastructure  Implementação do repositório (em memória)
PersonRegistry.Tests           Testes unitários (xUnit + Moq)
```

## Pré-requisitos

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)

## Executando a API

```bash
dotnet restore
dotnet run --project PersonRegistry.API
```

Em desenvolvimento, a documentação interativa (Scalar) fica disponível na raiz da aplicação.

## Executando os testes

```bash
dotnet test
```

## Autenticação

Um usuário administrador já vem configurado em `PersonRegistry.API/appsettings.json`:

- **Usuário:** `admin`
- **Senha:** `admin123`

1. Faça login para obter um token JWT:

   ```http
   POST /api/autenticacao/login
   Content-Type: application/json

   {
     "username": "admin",
     "password": "admin123"
   }
   ```

   Resposta:

   ```json
   { "token": "<jwt>", "tipo": "Bearer" }
   ```

2. Envie o token nas requisições aos endpoints de pessoa:

   ```http
   Authorization: Bearer <jwt>
   ```

A senha do admin é armazenada apenas como hash (BCrypt) em `AdminSettings:PasswordHash`, nunca em texto puro. Para trocar a senha, gere um novo hash em qualquer código C# com o pacote `BCrypt.Net-Next` (já referenciado em `PersonRegistry.Application`):

```csharp
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("nova-senha"));
```

e substitua o valor de `AdminSettings:PasswordHash` no `appsettings.json`.

> Em produção, evite manter segredos (`JwtSettings:Secret`, `AdminSettings:PasswordHash`) direto no `appsettings.json` do repositório — prefira variáveis de ambiente, `dotnet user-secrets` ou um cofre de segredos.

## Endpoints

### `POST /api/autenticacao/login`
Autentica e retorna um token JWT (válido por `JwtSettings:ExpirationHours`).

### `GET /api/pessoa`
Lista pessoas cadastradas. Suporta paginação via query string `skip` e `take`.

### `GET /api/pessoa/{codigo}`
Busca uma pessoa pelo código.

### `GET /api/pessoa/uf/{uf}`
Lista pessoas de uma UF (case-insensitive).

### `POST /api/pessoa`
Cadastra uma nova pessoa.

```json
{
  "nome": "Fulano de Tal",
  "cpf": "529.982.247-25",
  "uf": "GO",
  "dataNascimento": "1990-01-01"
}
```

Validações aplicadas: nome obrigatório (até 100 caracteres), data de nascimento obrigatória e não futura, CPF obrigatório/válido (dígito verificador) e não duplicado, UF obrigatória com 2 caracteres.

### `PUT /api/pessoa/{codigo}`
Atualiza uma pessoa existente (mesmas validações do cadastro).

### `DELETE /api/pessoa/{codigo}`
Remove uma pessoa pelo código.

> Todos os endpoints de `/api/pessoa` exigem autenticação (`[Authorize]`).

## Persistência

Os dados são armazenados em memória (`ConcurrentDictionary`, thread-safe) e são perdidos ao reiniciar a aplicação. Não há banco de dados configurado.
