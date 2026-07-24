# Sistema de Gestão de Tarefas — API
 
API RESTful para gerenciamento de tarefas, desenvolvida em **.NET** com **Entity Framework Core (InMemory)**, aplicando princípios de **SOLID**, injeção de dependência e arquitetura em camadas.
 
## Tecnologias utilizadas
 
- .NET 8+
- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- Swashbuckle (Swagger)
- xUnit
- Moq
## Como executar o projeto
 
### Pré-requisitos
 
- [.NET SDK](https://dotnet.microsoft.com/download) instalado
- Git
### Passo a passo
 
```bash
# 1. Clonar o repositório
git clone https://github.com/lucas0408/Teste-Dev-Jr.git
cd Teste-Dev-Jr
 
# 2. Restaurar as dependências
dotnet restore
 
# 3. Rodar os testes automatizados
dotnet test
 
# 4. Rodar a aplicação
dotnet run
```
 
Após rodar `dotnet run`, o terminal exibirá a porta em que a aplicação está escutando, por exemplo:
 
```
Now listening on: http://localhost:5032
```
 
Acesse a documentação interativa (Swagger) em:
 
```
http://localhost:5032/swagger
```
 
Por lá é possível testar todos os endpoints diretamente, sem necessidade de ferramentas externas como Postman.
 
## Estrutura do projeto
 
```
TesteDevjr/
├── Controllers/        → Camada de apresentação (recebe requisições HTTP)
├── DTOs/                → Objetos de transferência de dados (contrato da API)
├── Services/            → Camada de regras de negócio
├── Repositories/        → Camada de acesso a dados (EF Core)
├── Models/              → Entidades de domínio
├── Data/                → DbContext (EF Core)
├── Middlewares/          → Tratamento de erros global
├── Program.cs
└── tests/
    └── TesteDevjr.Tests/ → Testes unitários (xUnit + Moq)
```
 
## Arquitetura
 
O projeto adota uma **arquitetura em camadas** dentro de um único projeto (Controller → Service → Repository), com separação clara de responsabilidades por meio de pastas/namespaces, em vez de projetos `.csproj` separados. Essa escolha foi feita porque o escopo do desafio (uma única entidade, CRUD + filtro) não justifica a complexidade adicional de múltiplos assemblies — a separação lógica já é suficiente para atender aos requisitos de organização, modularidade e testabilidade.
 
- **Controllers**: recebem requisições HTTP, validam o `ModelState` e retornam os status codes apropriados. Não contêm regra de negócio.
- **DTOs**: definem o contrato público da API. O Controller nunca expõe diretamente as entidades de domínio (`Models`), evitando acoplamento entre a camada de apresentação e a camada de dados.
- **Services**: contêm a lógica de negócio, incluindo o mapeamento entre DTOs e entidades de domínio, e o logging de eventos relevantes (criação, atualização, exclusão).
- **Repositories**: encapsulam o acesso a dados via Entity Framework Core, isolando o restante da aplicação de detalhes de persistência.
- **Middlewares**: centralizam o tratamento de exceções não previstas, evitando repetição de `try/catch` em cada Controller e prevenindo exposição de detalhes internos (stack trace) ao cliente.
## Princípios SOLID aplicados
 
| Princípio | Onde é aplicado |
|---|---|
| **S** — Single Responsibility | Cada camada tem uma única responsabilidade: `TasksController` trata requisições HTTP, `TaskService` contém regras de negócio, `TaskRepository` lida com persistência, e `ExceptionHandlingMiddleware` trata erros. Nenhuma classe acumula mais de um motivo para mudar. |
| **O** — Open/Closed | Como `TaskService` depende da abstração `ITaskRepository`, é possível criar novas implementações (ex: um repositório para SQL Server) sem alterar o código existente do Service ou do Controller — a extensão ocorre por adição, não por modificação. |
| **L** — Liskov Substitution | Qualquer implementação de `ITaskRepository` pode substituir outra sem quebrar o comportamento esperado por quem a consome (`TaskService`), respeitando o contrato definido pela interface. |
| **D** — Dependency Inversion | `TasksController` depende de `ITaskService` e `TaskService` depende de `ITaskRepository`, nunca de implementações concretas. As implementações reais são injetadas apenas no `Program.cs` (Composition Root), via `AddScoped`. |
 
## Validações aplicadas
 
- **Título**: obrigatório, entre 3 e 200 caracteres, não pode conter apenas espaços em branco
- **Descrição**: opcional, até 1000 caracteres
- **Data de vencimento**: opcional, mas se informada, não pode ser anterior à data atual
- **Status**: obrigatório, deve ser um dos valores válidos (`Pendente`, `EmProgresso`, `Concluida`)
## Tratamento de erros e logging
 
A aplicação conta com um middleware global (`ExceptionHandlingMiddleware`) que captura qualquer exceção não tratada, registra o erro via `ILogger` e retorna uma resposta padronizada em JSON com status `500`, evitando expor detalhes internos da aplicação ao cliente.
 
O `TaskService` também registra eventos relevantes do fluxo de negócio (criação, atualização, exclusão e tentativas de operação em tarefas inexistentes) via `ILogger`, integrado ao sistema de logging padrão do ASP.NET Core.
 
## Testes
 
Os testes unitários cobrem a camada de serviço (`TaskService`), utilizando **Moq** para simular o `ITaskRepository` — isso é possível justamente pela aplicação do Dependency Inversion Principle, que permite testar a lógica de negócio isoladamente, sem depender de banco de dados real.
 
Para rodar os testes:
 
```bash
dotnet test
```
 
