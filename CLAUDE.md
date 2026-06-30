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
- A branch principal é `main`.

### Git

Convenções detalhadas na skill [`git-workflow`](.claude/skills/git-workflow/SKILL.md). Resumo:

- **Commits**: formato obrigatório `<conventional-commits>:<message>` (ex.: `feat:add culture middleware`). Inglês, imperativo, ≤ 50 caracteres, sem ponto final. **Nunca usar body** — commit é linha única.
- **Decidir o commit**: sempre analisar o Git antes (`status`, `diff`, arquivos não rastreados, `log`).
- **Múltiplas alterações**: separar em vários commits coesos e retornar uma tabela (`#` | Arquivos | Mensagem). Uma intenção só = um commit.
- **Branches**: kebab-case em inglês, padrão `add-<feature>` (ex.: `add-password-encryption`).
