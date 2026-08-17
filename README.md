# PersonRegistry

API REST em ASP.NET Core (.NET 10) para cadastro de pessoas, com autenticação via JWT. Organizada em Clean Architecture.

## Estrutura da solução

```
PersonRegistry.API             Controllers, Program.cs (startup), appsettings
PersonRegistry.Application     DTOs, serviços, validadores (FluentValidation), settings
PersonRegistry.Domain          Entidades, interfaces de repositório, validações de domínio (CPF, UF)
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

> **Nota sobre segredos:** `JwtSettings:Secret` e `AdminSettings:PasswordHash` estão commitados propositalmente no `appsettings.json` para que o projeto rode sem nenhuma configuração adicional (facilitando a avaliação deste desafio). Em um cenário real de produção, esses valores **não** deveriam ficar no controle de versão — a abordagem correta é usar `dotnet user-secrets` em desenvolvimento e variáveis de ambiente ou um cofre de segredos (Azure Key Vault, AWS Secrets Manager, etc.) em produção.

## Endpoints

### `POST /api/autenticacao/login`
Autentica e retorna um token JWT (válido por `JwtSettings:ExpirationHours`).

### `GET /api/pessoa`
Lista pessoas cadastradas, com paginação via query string `skip` e `take` (ambos opcionais; valores negativos retornam 400). A resposta inclui metadados de paginação:

```json
{
  "itens": [{ "codigo": 1, "nome": "Fulano de Tal", "cpf": "52998224725", "uf": "GO", "dataNascimento": "1990-01-01T00:00:00" }],
  "total": 1,
  "skip": 0,
  "take": null
}
```

### `GET /api/pessoa/{codigo}`
Busca uma pessoa pelo código.

### `GET /api/pessoa/uf/{uf}`
Lista pessoas de uma UF (case-insensitive).

### `POST /api/pessoa`
Cadastra uma nova pessoa e retorna o objeto salvo.

```json
{
  "nome": "Fulano de Tal",
  "cpf": "529.982.247-25",
  "uf": "GO",
  "dataNascimento": "1990-01-01"
}
```

Validações aplicadas:
- Nome obrigatório (até 100 caracteres).
- Data de nascimento obrigatória e não futura.
- CPF obrigatório, com dígito verificador válido e não duplicado (armazenado sempre sem máscara, apenas os dígitos, independente do formato enviado).
- UF obrigatória e válida (uma das 26 siglas de estado + DF).

### `PUT /api/pessoa/{codigo}`
Atualiza uma pessoa existente (mesmas validações do cadastro) e retorna o objeto atualizado.

### `DELETE /api/pessoa/{codigo}`
Remove uma pessoa pelo código.

> Todos os endpoints de `/api/pessoa` exigem autenticação (`[Authorize]`).

## Tratamento de erros

Erros de validação, autenticação e "não encontrado" seguem o padrão [`ProblemDetails`](https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5) (RFC 9110), por exemplo:

```json
{
  "title": "Erro de validação.",
  "status": 400,
  "errors": { "Cpf": ["O CPF informado é inválido."] }
}
```

## Persistência

Os dados são armazenados em memória (`ConcurrentDictionary`, thread-safe) e são perdidos ao reiniciar a aplicação. Não há banco de dados configurado.
