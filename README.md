## Como executar o projeto

### Pré-requisitos
- .NET 8 SDK instalado

### Backend (API)
1. Na pasta raiz do projeto, execute:
   ```
   dotnet restore
   dotnet run
   ```
2. O banco de dados SQLite (`salas.db`) é criado automaticamente na primeira execução, através das migrations já incluídas no projeto — não é necessário nenhum passo manual de banco de dados.
3. A API estará disponível em `http://localhost:5126` (verifique a porta exibida no terminal ao executar; ela também está configurada em `Properties/launchSettings.json`).

### Frontend
1. Abra o arquivo `index.html` diretamente no navegador (duplo clique ou `file://`). Não é necessário nenhum servidor web para o frontend — o CORS já está liberado na API para isso.
2. Faça login com as credenciais fixas:
   - **Email:** teste@teste.com
   - **Senha:** 123
3. Após o login, é possível cadastrar, listar, editar e excluir salas de reunião normalmente pela interface.


Ajusta a porta se a sua for diferente da `5126` ao testar.