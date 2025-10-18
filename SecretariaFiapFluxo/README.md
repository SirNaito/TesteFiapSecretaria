# Teste FIAP Secretaria
API RESTful para gerenciamento de **Alunos**, **Turmas** e **Matrículas** com autenticação via JWT.

# Features Adicionadas
- CRUD para **Alunos**, **Turmas**, **Matriculas** e **Admins**.
- Verificação de duplicidade para matriculas e cadastro de alunos
- Paginação ordenada alfabeticamente com limite de 10 itens por página
- Validações de CPF, Email, Data de Nascimento, Nome e Descrição de turma com limite mínimo de caractéres
- Validação de Senha com critérios de segurança (mínimo 8 caracteres, letras maiúsculas e minúsculas, números e símbolos)
- Swagger para testes e documentação.

# Stacks Utilizadas
- .NET 8
- Entity Framework (Database First conforme solicitado)
- SQL Server
- JWT (JSON Web Token)
- Swagger (OpenAPI)

# Como Rodar
1. Execute o script dump.sql para criar o banco com as tabelas e inserir o admin inicial.
2. Rode o projeto via terminal no vscode (ou pelo visual studio)

# Comandos via terminal vscode
dotnet restore
dotnet build
dotnet run --project SecretariaFiapFluxo.Api

Url do Swagger:
http://localhost:5002/swagger

Login do admin inicial
Email: admin@fiap.com.br
Senha: Admin123