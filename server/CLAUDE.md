# Backend (`server/`)

API .NET 10 em **Clean Architecture**. Idioma e convenções herdam do [`CLAUDE.md`](../CLAUDE.md) da raiz (docs/comentários em pt-br; código, commits e branches em inglês).

## Stack

- .NET **10** (`net10.0`), `Nullable` e `ImplicitUsings` habilitados em todos os projetos.
- Solução: `Solution.slnx` (formato slnx, não `.sln`).
- Autenticação **JWT** (`Microsoft.AspNetCore.Authentication.JwtBearer`).
- Validação com **FluentValidation**.
- Hash de senha com **BCrypt.Net-Next**.
- Testes com **xUnit** + `Microsoft.AspNetCore.Mvc.Testing`.

## Camadas e dependências

Cada camada é um projeto separado. A direção das dependências aponta sempre para o centro (`Domain`):

| Projeto | Papel | Depende de |
|---------|-------|-----------|
| `Api` | Controllers, filtros, extensões de bootstrap, `Program` | `Application`, `Infrastructure` |
| `Application` | Casos de uso (`UseCases`) e validadores | `Communication`, `Domain`, `Exception` |
| `Infrastructure` | Implementações de segurança (JWT, BCrypt), acesso externo | `Domain` |
| `Communication` | DTOs de request/response (contratos da API) | — |
| `Domain` | Entidades, enums, interfaces de segurança | — |
| `Exception` | Exceções customizadas (`ExceptionBase`) | — |
| `WebApi.Test` | Testes de integração (referencia `Api`) | `Api` |

Regra: `Domain` e `Communication` não dependem de ninguém. Não criar dependência de `Domain` → `Application`/`Infrastructure`.

## Convenções de código

- **Primary constructors** para injeção de dependência (ex.: `LoginUseCase(IAccessTokenGenerator tokenService, IPasswordEncripter passwordEncripter)`).
- **Registro de DI por camada**, via extension method próprio:
  - `Application` → `AddApplication()` (registra casos de uso).
  - `Infrastructure` → `AddInfrastructure(configuration)` (registra serviços de segurança).
  - `Api` → `AddAuthenticationConfigs(configuration)` (JWT).
  - Encadeados em `Program.cs`. Ao adicionar serviço novo, registrar na extension da camada dele — não no `Program`.
- **Casos de uso**: interface `I<Nome>UseCase` + classe, em `Application/UseCases/<Área>/`. Injetar via `[FromServices]` no controller, nunca instanciar com `new`.
- **Interfaces de abstração de infra** vivem em `Domain/Security/` (ex.: `IAccessTokenGenerator`, `IPasswordEncripter`); as implementações em `Infrastructure/Security/`.
- **DTOs** em `Communication/Request` e `Communication/Response` — controllers e use cases trocam DTOs, não entidades.

## Tratamento de erros

- Exceções de negócio herdam de `ExceptionBase` (em `Exception/`), com `StatusCode` e `GetErrors()`.
- `ExceptionFilter` (registrado globalmente em `Program.cs`) converte:
  - `ExceptionBase` → status + `ResponseErrorJson` com as mensagens.
  - Qualquer outra → `500` com "Erro desconhecido".
- Para um novo erro mapeado a HTTP, criar classe herdando `ExceptionBase` (ex.: `UnauthorizedException`, `ErrorOnValidationException`), não retornar status manualmente no controller.

## Como rodar

```bash
cd server
dotnet restore
dotnet run --project Api
```

A API sobe em `https://localhost:9000` (ver `Api/Properties/launchSettings.json`).

## Testes

```bash
cd server
dotnet test
```

- Projeto de teste: `WebApi.Test` (testes de integração via `WebApplicationFactory<Api.Program>`).
- CI roda `restore` → `build --no-restore` → `test --no-build` apenas em `pull_request` (`.github/workflows/tests.yml`).

## Configuração

- Seção `Jwt` (`Issuer`, `Audience`, `Key`) lida em `AddAuthenticationConfigs`. **Não** comitar chave real em `appsettings.json`.
- Credenciais de login hoje estão hardcoded em `LoginUseCase` (`admin@admin.com` / `123456`) — scaffold, ainda sem persistência. Substituir por repositório quando houver banco.
