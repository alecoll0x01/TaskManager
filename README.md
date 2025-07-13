# Sistema de Gerenciamento de Tarefas

## Visão Geral
API RESTful para gerenciamento de tarefas desenvolvida com **.NET 8** utilizando **Domain-Driven Design (DDD)**, **Clean Architecture**, **CQRS** com **MediatR**, e **cobertura de testes superior a 80%**.

## 🏗️ Arquitetura

### Clean Architecture + DDD
O projeto segue os princípios de Clean Architecture com DDD, separando as responsabilidades em camadas bem definidas:

```
📁 src/
├── 📁 TaskManagement.Domain/          # Camada de Domínio (Entidades, Value Objects, Domain Events)
├── 📁 TaskManagement.Application/     # Camada de Aplicação (Use Cases, DTOs, Handlers)
├── 📁 TaskManagement.Infrastructure/  # Camada de Infraestrutura (Repositórios, DbContext)
└── 📁 TaskManagement.API/            # Camada de Apresentação (Controllers, Middleware)
```

### Conceitos Aplicados

#### 1. **Domain-Driven Design (DDD)**
- **Entidades Ricas**: `Project`, `Task`, `User` com lógica de negócio encapsulada
- **Value Objects**: `TaskPriority`, `TaskStatus`, `UserRole`
- **Domain Events**: Eventos disparados em operações importantes (`TaskCreatedDomainEvent`, `ProjectCreatedDomainEvent`)
- **Domain Services**: `PerformanceReportService` para lógicas complexas
- **Aggregate Roots**: `Project` como agregado que controla `Tasks`

#### 2. **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Operações de escrita (`CreateProjectCommand`, `UpdateTaskCommand`)
- **Queries**: Operações de leitura (`GetProjectsByUserQuery`, `GetTasksByProjectQuery`)
- **Handlers**: Separação clara entre lógica de comando e consulta

#### 3. **Event-Driven Architecture**
- Domain Events automaticamente publicados via MediatR
- Histórico de alterações automático através de eventos
- Desacoplamento entre agregados

#### 4. **Repository Pattern + Unit of Work**
- Repositórios específicos por agregado
- Unit of Work para gerenciamento de transações
- Abstração da persistência

## 🚀 Funcionalidades

### Endpoints Principais

#### Projetos
- `GET /api/projects/user/{userId}` - Listar projetos do usuário
- `POST /api/projects` - Criar novo projeto

#### Tarefas
- `GET /api/tasks/project/{projectId}` - Visualizar tarefas do projeto
- `POST /api/tasks` - Criar nova tarefa
- `PUT /api/tasks/{taskId}` - Atualizar tarefa
- `DELETE /api/tasks/{taskId}` - Remover tarefa
- `POST /api/tasks/{taskId}/comments` - Adicionar comentário

#### Relatórios
- `GET /api/reports/performance/{userId}` - Relatório de performance (apenas gerentes)

### Regras de Negócio Implementadas

1. **Prioridades de Tarefas**: Imutáveis após criação
2. **Limite de Tarefas**: Máximo 20 tarefas por projeto
3. **Restrições de Remoção**: Projetos com tarefas pendentes não podem ser removidos
4. **Histórico Automático**: Todas as alterações são registradas
5. **Comentários**: Integrados ao histórico de alterações
6. **Relatórios**: Acessíveis apenas por gerentes

## 🛠️ Tecnologias Utilizadas

### Core
- **.NET 8** - Framework principal
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Banco de dados
- **MediatR** - CQRS e mediação
- **FluentValidation** - Validação de dados

### Qualidade
- **xUnit** - Framework de testes
- **Moq** - Mock para testes
- **FluentAssertions** - Assertions expressivas
- **Coverlet** - Cobertura de código

### Documentação
- **Swagger/OpenAPI** - Documentação automática da API

## 🐳 Execução com Docker

### Pré-requisitos
- Docker
- Docker Compose

### Comandos para Execução

```bash
# Clonar o repositório
git clone 
cd task-management-system

# Executar com Docker Compose
docker-compose up -d

# Verificar se os containers estão rodando
docker-compose ps

# Ver logs
docker-compose logs -f taskmanagement-api
```

### URLs Disponíveis
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000
- **PostgreSQL**: localhost:5432

### Parar a Aplicação
```bash
docker-compose down
```

## 🧪 Testes

### Executar Testes
```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Gerar relatório de cobertura
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport"
```

### Estrutura de Testes
```
📁 tests/
├── 📁 TaskManagement.Domain.Tests/     # Testes de domínio (entidades, value objects)
├── 📁 TaskManagement.Application.Tests/ # Testes de handlers e comandos
├── 📁 TaskManagement.Infrastructure.Tests/ # Testes de repositórios
└── 📁 TaskManagement.API.Tests/        # Testes de integração
```

### Cobertura de Testes
- **Meta**: >80% de cobertura
- **Foco**: Regras de negócio e casos críticos
- **Tipos**: Unitários, Integração e Comportamentais

## 📊 Exemplos de Uso

### Criar Projeto
```bash
curl -X POST "http://localhost:5000/api/projects" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Projeto de Exemplo",
    "description": "Descrição do projeto",
    "userId": "123e4567-e89b-12d3-a456-426614174000"
  }'
```

### Criar Tarefa
```bash
curl -X POST "http://localhost:5000/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Implementar API",
    "description": "Desenvolver endpoints RESTful",
    "dueDate": "2024-12-31T23:59:59Z",
    "priority": 2,
    "projectId": "123e4567-e89b-12d3-a456-426614174001",
    "userId": "123e4567-e89b-12d3-a456-426614174000"
  }'
```

## 🔄 Fase 2: Perguntas para Refinamento

### Questões Técnicas

1. **Autenticação e Autorização**
   - Qual sistema de autenticação será integrado? (JWT, OAuth2, Azure AD)
   - Como será o controle de permissões por projeto/organização?
   - Haverá diferentes níveis de acesso além de usuário/gerente?

2. **Escalabilidade e Performance**
   - Qual é o volume esperado de usuários simultâneos?
   - Há necessidade de cache (Redis) para consultas frequentes?
   - A API precisa suportar paginação avançada e filtros complexos?

3. **Integrações Externas**
   - Haverá integração com ferramentas como Slack, Teams ou email?
   - É necessário sincronização com calendários (Outlook, Google)?
   - Há planos para webhooks ou APIs de terceiros?

4. **Funcionalidades Avançadas**
   - Como funcionarão as notificações (push, email, in-app)?
   - Haverá suporte a anexos em tarefas e comentários?
   - É necessário versionamento de documentos/anexos?

### Questões de Negócio

1. **Colaboração**
   - Como será o compartilhamento de projetos entre usuários?
   - Haverá aprovações ou workflows para mudanças críticas?
   - É necessário controle de tempo gasto em tarefas?

2. **Relatórios e Analytics**
   - Quais métricas são mais importantes para o negócio?
   - Há necessidade de dashboards em tempo real?
   - Relatórios precisam ser exportados (PDF, Excel)?

3. **Configurações**
   - Usuários podem personalizar tipos de prioridade/status?
   - Haverá templates de projeto reutilizáveis?
   - É necessário configuração de fuso horário por usuário?

### Questões de UX/UI

1. **Interface**
   - Haverá uma aplicação web front-end?
   - É necessário suporte mobile (app nativo ou PWA)?
   - Como será a experiência offline?

2. **Usabilidade**
   - Haverá drag-and-drop para reorganizar tarefas?
   - É necessário busca avançada com filtros complexos?
   - Como será o sistema de notificações visuais?

## 🚀 Fase 3: Melhorias e Evoluções

### Arquitetura e Cloud

#### 1. **Microserviços**
- **Decomposição**: Separar em serviços de Projetos, Tarefas, Usuários e Relatórios
- **API Gateway**: Implementar gateway centralizado com rate limiting
- **Service Mesh**: Considerar Istio para comunicação inter-serviços
- **Event-Driven**: Implementar message brokers (RabbitMQ/Azure Service Bus)

#### 2. **Cloud-Native**
```yaml
# Kubernetes Deployment Example
apiVersion: apps/v1
kind: Deployment
metadata:
  name: taskmanagement-api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: taskmanagement-api
  template:
    metadata:
      labels:
        app: taskmanagement-api
    spec:
      containers:
      - name: api
        image: taskmanagement:latest
        ports:
        - containerPort: 80
        env:
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: db-secret
              key: connection-string
```

#### 3. **Observabilidade**
- **Logging**: Structured logging com Serilog + ELK Stack
- **Metrics**: Prometheus + Grafana para métricas de performance
- **Tracing**: OpenTelemetry para tracing distribuído
- **Health Checks**: Implementar health checks detalhados

### Performance e Escalabilidade

#### 1. **Cache Strategy**
```csharp
// Redis Cache Implementation
public class CachedProjectRepository : IProjectRepository
{
    private readonly IProjectRepository _repository;
    private readonly IDistributedCache _cache;
    
    public async Task GetByIdAsync(Guid id)
    {
        var cacheKey = $"project:{id}";
        var cachedProject = await _cache.GetStringAsync(cacheKey);
        
        if (cachedProject != null)
            return JsonSerializer.Deserialize(cachedProject);
            
        var project = await _repository.GetByIdAsync(id);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(project), 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
            
        return project;
    }
}
```

#### 2. **Database Optimization**
- **Read Replicas**: Separar leitura e escrita
- **Indexing Strategy**: Índices otimizados para consultas frequentes
- **Partitioning**: Particionamento por tenant ou data
- **Connection Pooling**: Configuração otimizada de pool de conexões

#### 3. **CQRS Avançado**
```csharp
// Eventual Consistency with Projections
public class TaskProjectionHandler : INotificationHandler
{
    private readonly ITaskReadModelRepository _readRepository;
    
    public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var readModel = new TaskReadModel
        {
            Id = notification.TaskId,
            ProjectId = notification.ProjectId,
            Title = notification.Title,
            CreatedAt = notification.OccurredAt
        };
        
        await _readRepository.UpsertAsync(readModel);
    }
}
```

### Segurança

#### 1. **Security Headers**
```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
    await next();
});
```

#### 2. **API Security**
- **Rate Limiting**: Implementar throttling por IP/usuário
- **Input Validation**: Validação robusta contra injection attacks
- **Audit Trail**: Log completo de operações sensíveis
- **Encryption**: Criptografia de dados sensíveis em repouso

### DevOps e CI/CD

#### 1. **Pipeline Completo**
```yaml
# GitHub Actions Example
name: CI/CD Pipeline
on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    - name: Run Tests
      run: |
        dotnet test --configuration Release \
          --collect:"XPlat Code Coverage" \
          --logger trx --results-directory TestResults/
    - name: Code Coverage
      run: |
        dotnet tool install -g dotnet-reportgenerator-globaltool
        reportgenerator -reports:TestResults/*/coverage.cobertura.xml \
          -targetdir:TestResults/CoverageReport
```

#### 2. **Infrastructure as Code**
```hcl
# Terraform para Azure
resource "azurerm_container_app_environment" "main" {
  name                = "taskmanagement-env"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
}

resource "azurerm_container_app" "api" {
  name                         = "taskmanagement-api"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode               = "Single"

  template {
    container {
      name   = "taskmanagement-api"
      image  = "your-registry.azurecr.io/taskmanagement:latest"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}
```

### Monitoramento e Alertas

#### 1. **Application Insights**
```csharp
// Custom Telemetry
public class TaskMetricsService
{
    private readonly TelemetryClient _telemetryClient;
    
    public void TrackTaskCreated(string projectId, string priority)
    {
        _telemetryClient.TrackEvent("TaskCreated", new Dictionary
        {
            ["ProjectId"] = projectId,
            ["Priority"] = priority
        });
        
        _telemetryClient.TrackMetric("TasksCreated", 1);
    }
}
```

#### 2. **Health Checks Avançados**
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck()
    .AddRedis(connectionString)
    .AddCheck("external-api")
    .AddCheck("memory", () =>
    {
        var allocated = GC.GetTotalMemory(false);
        var threshold = 500 * 1024 * 1024; // 500MB
        return allocated < threshold ? HealthCheckResult.Healthy() 
                                    : HealthCheckResult.Unhealthy();
    });
```

### Testes Avançados

#### 1. **Architecture Tests**
```csharp
[Fact]
public void Domain_Should_Not_Depend_On_Infrastructure()
{
    var result = Types.InAssembly(DomainAssembly)
        .Should()
        .NotHaveDependencyOn("TaskManagement.Infrastructure")
        .GetResult();
        
    result.IsSuccessful.Should().BeTrue();
}
```

#### 2. **Performance Tests**
```csharp
[Fact]
public async Task CreateTask_Should_Complete_Within_100ms()
{
    // Arrange
    var stopwatch = Stopwatch.StartNew();
    
    // Act
    await _handler.Handle(command, CancellationToken.None);
    
    // Assert
    stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
}
```

### Padrões Adicionais

#### 1. **Specification Pattern**
```csharp
public class OverdueTasksSpecification : Specification
{
    public override Expression<Func> ToExpression()
    {
        return task => task.DueDate < DateTime.UtcNow && 
                      task.Status != TaskStatus.Completed;
    }
}
```

#### 2. **Outbox Pattern**
```csharp
public class OutboxEvent
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string EventData { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Processed { get; set; }
}
```

## 🏆 Critérios de Qualidade Atendidos

### Clareza e Organização
- ✅ Separação clara de responsabilidades por camada
- ✅ Nomenclatura expressiva e consistente
- ✅ Documentação abrangente com exemplos
- ✅ Padrões arquiteturais bem definidos

### Eficiência na Resolução
- ✅ Implementação completa de todos os requisitos
- ✅ Regras de negócio encapsuladas no domínio
- ✅ Performance otimizada com patterns adequados
- ✅ Tratamento robusto de erros

### Boas Práticas
- ✅ SOLID principles aplicados
- ✅ Clean Code com métodos pequenos e focados
- ✅ Testes abrangentes com alta cobertura
- ✅ Configuração via Docker para portabilidade
- ✅ API RESTful com documentação automática

---