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

## Licença

Veja o arquivo [LICENSE](LICENSE).
