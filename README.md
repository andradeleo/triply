# Triply

Aplicação web dividida em dois projetos: um cliente Angular e uma API .NET.

## Estrutura

| Projeto | Tecnologia | Porta |
|---------|-----------|-------|
| `client/` | Angular | 4200 |
| `server/` | API .NET | 9000 |

## Pré-requisitos

- [Node.js](https://nodejs.org/) e npm
- [Angular CLI](https://angular.dev/tools/cli)
- [.NET SDK](https://dotnet.microsoft.com/download)

## Como rodar

### Backend (`server/`)

```bash
cd server
dotnet restore
dotnet run
```

A API sobe em `http://localhost:9000`.

### Frontend (`client/`)

```bash
cd client
npm install
ng serve
```

O cliente fica disponível em `http://localhost:4200` e consome a API na porta 9000.

## Skills do Claude

O projeto inclui skills para o [Claude Code](https://claude.com/claude-code) em `.claude/skills/`, que automatizam convenções do repositório.

| Skill | Funcionalidade |
|-------|----------------|
| [`git-workflow`](.claude/skills/git-workflow/SKILL.md) | Convenções de Git: nomeação de branches, formato de commit (`<conventional-commits>:<message>`, sem body) e separação de múltiplas alterações em commits coesos — analisando o estado do repositório antes de sugerir. |

## Licença

Veja o arquivo [LICENSE](LICENSE).
