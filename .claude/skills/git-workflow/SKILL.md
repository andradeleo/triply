---
name: git-workflow
description: Convenções de Git do triply — como nomear branches, formatar mensagens de commit e decidir o conteúdo do commit analisando o estado do repositório. Use quando o usuário pedir para criar/sugerir um commit, nomear uma branch, ou perguntar sobre as convenções de Git do projeto.
---

# Fluxo de Git do triply

Convenções de versionamento do repositório `triply`. Siga ao sugerir branches e commits.

## Idioma

- **Mensagens de commit e nomes de branch**: sempre em **inglês**.
- Conversa com o usuário pode ser em pt-br, mas o artefato Git é em inglês.

## Como decidir o commit (analisar o Git primeiro)

Antes de sugerir qualquer mensagem, **analise o estado do repositório**:

1. `git status --porcelain` — ver arquivos modificados, adicionados e não rastreados.
2. `git diff` — ver mudanças nos arquivos rastreados.
3. `ls -R <dir>` nos diretórios não rastreados (`??`) — entender arquivos novos que o diff não mostra.
4. `git log --oneline -15` — ver o estilo do histórico e padrões de nomeação.
5. `git branch -a` — ver padrão de nomeação das branches existentes.

A partir disso, identifique **a intenção central** da mudança (não liste arquivo por arquivo). A mensagem descreve o *porquê*/efeito, não o *como*.

## Formato do commit — Conventional Commits

Padrão **obrigatório**: `<conventional-commits>:<message>`

- Sempre nesse formato. Sem exceção.
- Subject em inglês, no imperativo, minúsculo, **≤ 50 caracteres**, sem ponto final.
- **Nunca usar body.** Commit é uma única linha.

### Tipos usados no histórico

| Tipo | Quando usar | Exemplo do repo |
|------|-------------|-----------------|
| `feat` | Nova funcionalidade | `feat: hash password with BCrypt on login` |
| `refactor` | Mudança sem alterar comportamento | `refactor: register use cases via DI instead of manual instantiation` |
| `docs` | Documentação | `docs: add backend CLAUDE.md and link it from root` |
| `ci` | Pipeline / GitHub Actions | `ci: run tests workflow only on pull requests` |
| `test` | Testes | `test: add xUnit integration tests` |
| `chore` | Tarefas de manutenção / scaffold | `chore: scaffold project structure` |

Outros tipos válidos quando aplicável: `fix` (correção de bug), `style`, `perf`.

## Múltiplas alterações → múltiplos commits

Se ao analisar o Git identificar **várias mudanças distintas** (intenções diferentes), **não juntar tudo num commit só**. Separar em vários commits que façam sentido (cada commit = uma intenção coesa).

Nesse caso, retornar uma **tabela** descrevendo cada commit:

| # | Arquivos | Mensagem |
|---|----------|----------|
| 1 | `caminho/arquivo-a.cs`, `caminho/arquivo-b.cs` | `feat:add culture middleware` |
| 2 | `caminho/arquivo-c.csproj` | `chore:configure resource embedding` |

- Coluna **Arquivos**: quais arquivos entram no commit.
- Coluna **Mensagem**: no formato `<conventional-commits>:<message>`.
- Ordenar commits por dependência lógica (base primeiro).
- Se for uma só intenção, um único commit basta (sem tabela).

## Nomeação de branch

- Em inglês, **kebab-case**.
- Padrão do histórico: `add-<feature>` para novas funcionalidades.
  - Exemplos reais: `add-password-encryption`, `add-validation-and-custom-execptions`.
- Branch principal: `main`.
- Para outros casos, use prefixo descritivo coerente com o tipo: `fix-<bug>`, `refactor-<alvo>`.

## Pull Requests

- O histórico integra via merge de PR (`Merge pull request #N from andradeleo/<branch>`).
- Trabalhe em branch, abra PR contra `main`.

## Checklist ao sugerir um commit

1. Analisei `status`, `diff` e arquivos não rastreados.
2. Identifiquei a(s) intenção(ões) da mudança.
3. Se houver várias intenções, separei em commits e retornei a tabela.
4. Escolhi o `type` correto para cada commit.
5. Formato `<conventional-commits>:<message>`, inglês, imperativo, ≤ 50 chars, sem ponto final.
6. Sem body em nenhum commit.
7. Sugeri nome de branch em `add-<feature>` (ou prefixo adequado) quando relevante.
