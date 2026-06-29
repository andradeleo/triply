## Idioma

- **Este arquivo (CLAUDE.md), comentários de código e documentação** (ex.: `README.md`): português do Brasil (pt-br).
- **Código, mensagens de commit e nomes de branch**: inglês.

## Visão geral

O `triply` é uma aplicação dividida em dois projetos:

- `client/` — frontend **Angular**, roda na porta **4200**.
- `server/` — backend **API .NET**, roda na porta **9000**. Detalhes específicos do backend em [`server/CLAUDE.md`](server/CLAUDE.md).

## Portas

| Projeto | Tecnologia | Porta |
|---------|-----------|-------|
| `client/` | Angular | 4200 |
| `server/` | .NET API | 9000 |

## Convenções

- Commits e branches em inglês.
- Manter mensagens de commit no padrão do histórico existente (ex.: `chore: scaffold project structure`) — Conventional Commits.
- A branch principal é `main`.
