# FiapPostTechUsers - Microserviço de Usuários

Microserviço de gerenciamento de usuários e autenticação com **observabilidade completa** implementando:
- 👤 **Gestão de Usuários**: CRUD completo com validações e autenticação
- 📊 **Distributed Tracing**: OpenTelemetry + Jaeger para rastreamento distribuído
- 📈 **Monitoramento**: Prometheus + Grafana + ELK Stack
- 🚀 **Infraestrutura**: Docker containerizado com health checks

## 🎯 **Como o Microserviço Users está Funcionando**

O sistema utiliza arquitetura limpa para:
- **👤 Autenticação**: JWT tokens com refresh token support
- **🔐 Autorização**: Role-based access control (RBAC)
- **📝 Validações**: FluentValidation para regras de negócio
- **🔄 Event Sourcing**: Eventos publicados via RabbitMQ
- **⚡ Performance**: Entity Framework com otimizações

### **🏗️ Arquitetura do Microserviço**
- **Domain Layer**: Entidades, enums e regras de negócio
- **Application Layer**: Serviços, DTOs, validações e mappers
- **Infrastructure Layer**: Repositórios, banco de dados e integrações
- **API Layer**: Controllers, middleware e configurações
- **Event Integration**: RabbitMQ para comunicação assíncrona
- **Observabilidade**: OpenTelemetry integration completa

### **🔍 Observabilidade & Distributed Tracing**
- **OpenTelemetry**: Instrumentação automática de HTTP requests e ASP.NET Core
- **Jaeger**: Visualização de traces distribuídos na porta 16686
- **Service Name**: "Users.Api" para identificação no tracing
- **Trace Correlation**: Correlação automática entre microserviços
- **Performance Monitoring**: Medição de latência e identificação de gargalos
- **Event Tracing**: Rastreamento de eventos RabbitMQ

## 🚀 **Como Iniciar**

### **1. Pré-requisitos**
```bash
# Ferramentas necessárias
- Docker Desktop
- .NET 8 SDK
- Git
```

### **2. Infraestrutura (Docker)**
```bash
# 1. Navegar para o diretório de infraestrutura
cd ../FiapPostTechDocker

# 2. Subir infraestrutura completa
docker-compose up -d sqlserver elasticsearch kibana logstash prometheus grafana jaeger rabbitmq

# 3. Verificar containers rodando
docker ps
# Deve mostrar: sqlserver (1433), elasticsearch (9200), jaeger (16686), prometheus (9090), etc.

# 4. Testar serviços principais
curl http://localhost:9200     # Elasticsearch
curl http://localhost:16686    # Jaeger UI
curl http://localhost:9090     # Prometheus
curl http://localhost:3000     # Grafana
```

### **3. Aplicação (.NET)**
```bash
# 1. Navegar para a API
cd src/Users.Api

# 2. Restaurar pacotes
dotnet restore

# 3. Executar aplicação
dotnet run
# Aplicação disponível em: http://localhost:5000
# Swagger disponível em: http://localhost:5000/swagger
```

### **4. Inicialização Automática**
A aplicação faz automaticamente:
- ✅ **Aplicação de migrations**: DatabaseStructure + seed data
- ✅ **Configuração JWT**: Keys e validação automática
- ✅ **Setup RabbitMQ**: Queues e exchanges configurados
- ✅ **Health checks**: Monitoramento de SQL Server + RabbitMQ

## 🔐 **Sistema de Autenticação**

### **Registro de Usuário**
```http
POST /api/v1/users/register
```
**Função**: Registra novo usuário no sistema com validações completas.
**Validações**: Email único, senha forte, dados obrigatórios.
**Eventos**: Publica UserCreated via RabbitMQ para outros microserviços.

### **Login de Usuário**
```http
POST /api/v1/users/login
```
**Função**: Autentica usuário e retorna JWT token + refresh token.
**Response**: Access token (15min) + Refresh token (7 dias).
**Segurança**: Hash bcrypt para senhas.

### **Refresh Token**
```http
POST /api/v1/users/refresh
```
**Função**: Renova access token usando refresh token válido.
**Rotação**: Refresh token é rotacionado a cada uso.

### **Logout**
```http
POST /api/v1/users/logout
```
**Função**: Invalida refresh token no servidor.
**Segurança**: Blacklist de tokens comprometidos.

## 👤 **Gestão de Usuários**

### **Perfil do Usuário**
```http
GET /api/v1/users/profile
```
**Função**: Retorna dados do perfil do usuário autenticado.
**Autorização**: Requer JWT token válido.

### **Atualizar Perfil**
```http
PUT /api/v1/users/profile
```
**Função**: Atualiza dados do perfil (nome, email, etc.).
**Validações**: Email único se alterado, dados obrigatórios.

### **Alterar Senha**
```http
PUT /api/v1/users/change-password
```
**Função**: Permite alteração de senha com validação da senha atual.
**Segurança**: Validação da senha atual + nova senha forte.

### **Listar Usuários (Admin)**
```http
GET /api/v1/users
```
**Função**: Lista usuários com paginação (apenas administradores).
**Autorização**: Requer role Admin.

## 📊 **Eventos e Integrações**

### **Eventos Publicados**
- **UserCreated**: Quando usuário se registra
- **UserUpdated**: Quando perfil é atualizado
- **UserLoggedIn**: Quando usuário faz login

### **Consumidores de Eventos**
O microserviço consome eventos de outros serviços para:
- Sincronização de dados
- Auditoria de ações
- Notificações

## ⚙️ **Características Técnicas**

### **🎯 Arquitetura**
- **Clean Architecture**: Separação clara de responsabilidades
- **CQRS Pattern**: Command/Query separation para operações
- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Transações consistentes
- **AutoMapper**: Mapeamento automático entre DTOs e entidades

### **🔐 Segurança**
- **JWT Authentication**: Tokens stateless com claims
- **Role-based Authorization**: Controle granular de acesso
- **Password Hashing**: Bcrypt com salt automático
- **Token Rotation**: Refresh tokens com rotação
- **CORS Policy**: Configuração segura para frontend

### **📊 Validações e DTOs**
- **FluentValidation**: Validações complexas e customizadas
- **Request DTOs**: Entrada de dados validada
- **Response DTOs**: Saída controlada sem exposição de dados internos
- **Error Handling**: Middleware global para tratamento de exceções

## 🔍 **Monitoramento e Observabilidade**

### **Health Checks e Observabilidade**
- **`/health`**: Status geral da aplicação + dependências
- **`/health/ready`**: Verificação específica de conectividade
- **`/health/live`**: Liveness probe para containers
- **`/metrics`**: Métricas Prometheus para observabilidade

### **🔍 Distributed Tracing Endpoints**
- **Jaeger UI**: `http://localhost:16686` - Visualização de traces
- **Service Name**: "Users.Api" - Identificação no Jaeger
- **Trace Correlation**: Automática em todas as HTTP requests
- **Custom Spans**: Implementados para operações críticas de autenticação

### **📊 Dashboards de Monitoramento**
- **Grafana**: `http://localhost:3000` (admin/admin)
- **Prometheus**: `http://localhost:9090` - Métricas coletadas
- **Kibana**: `http://localhost:5601` - Logs centralizados
- **RabbitMQ**: `http://localhost:15672` (guest/guest)

### **📝 Logs Estruturados**
O sistema gera logs estruturados para:
- **Autenticação**: Login, logout, falhas de autenticação
- **Operações CRUD**: Criação, atualização de usuários
- **Eventos**: Publicação e consumo via RabbitMQ
- **Performance**: Tempo de resposta das operações

## 🔧 **Configuração e Deploy**

### **Variáveis de Ambiente**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SQL Server connection"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "Users.Api",
    "Audience": "Users.Api",
    "ExpirationMinutes": 15
  },
  "RabbitMQ": {
    "ConnectionString": "amqp://guest:guest@rabbitmq:5672/"
  }
}
```

### **Docker Setup**
```yaml
users.api:
  image: usersapi
  container_name: FiapUsers
  ports: ["5000:80"]
  networks: [postech-network]
  depends_on: [sqlserver, rabbitmq]
```

## 🧪 **Verificação de Funcionamento**

### **Testando os Endpoints**

**Swagger UI**: Acesse `http://localhost:5000/swagger` para testar todos os endpoints interativamente.

**Principais Testes**:
- **Registro**: `POST /api/v1/users/register` com dados válidos
- **Login**: `POST /api/v1/users/login` com credenciais
- **Perfil**: `GET /api/v1/users/profile` com JWT token
- **Health**: `GET /health` para verificar status

### **Teste de Autenticação Completo**
```bash
# 1. Registrar usuário
curl -X POST http://localhost:5000/api/v1/users/register \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com","password":"Test123!"}'

# 2. Fazer login
curl -X POST http://localhost:5000/api/v1/users/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}'

# 3. Usar token retornado para acessar perfil
curl -X GET http://localhost:5000/api/v1/users/profile \
  -H "Authorization: Bearer {seu-jwt-token}"
```

## 🚨 **Troubleshooting Completo**

### **Problemas de Autenticação**

**JWT Token inválido**:
1. Verificar se token não expirou
2. Validar formato do header: `Authorization: Bearer {token}`
3. Verificar configuração JwtSettings no appsettings.json

**Problemas de Registro**:
1. Verificar se email já existe
2. Validar força da senha (mínimo 8 caracteres, maiúscula, número, símbolo)
3. Verificar conectividade com SQL Server

### **Problemas de Infraestrutura**

**RabbitMQ não conecta**:
1. Verificar container: `docker ps | grep rabbitmq`
2. Testar conectividade: `curl http://localhost:15672`
3. Verificar logs: `docker logs rabbitmq`

**Traces não aparecem no Jaeger**:
1. Verificar Jaeger: `curl http://localhost:16686`
2. Verificar configuração OpenTelemetry nos logs da aplicação
3. Fazer requests para gerar traces

## ✅ **Requisitos FIAP Tech Challenge Atendidos**

### **Gestão de Usuários - 100% Implementado**
- ✅ **CRUD Completo**: Registro, atualização, consulta de usuários
- ✅ **Autenticação JWT**: Login/logout com tokens seguros
- ✅ **Autorização RBAC**: Controle de acesso baseado em roles
- ✅ **Validações**: FluentValidation para regras de negócio

### **Distributed Tracing - 100% Implementado**
- ✅ **OpenTelemetry**: Instrumentação automática completa
- ✅ **Jaeger**: Coleta e visualização de traces distribuídos
- ✅ **Service Correlation**: Rastreamento entre microserviços
- ✅ **Performance Monitoring**: Identificação de gargalos

### **Funcionalidades Extras Implementadas**
- 🔐 **Segurança Avançada**: Hash bcrypt, token rotation, CORS
- 📊 **Observabilidade Completa**: ELK + Prometheus + Grafana + Jaeger
- 🔄 **Event Sourcing**: RabbitMQ para comunicação assíncrona
- ⚙️ **Clean Architecture**: Código limpo e testável
- 🚀 **Performance**: Operações otimizadas com monitoramento

## 👥 **Ecossistema FIAP Tech Challenge**

Este projeto faz parte da arquitetura de microserviços:
- **👤 FiapPosTechUsers**: Microserviço de usuários e autenticação (este projeto)
- **🎮 FiapPosTechGames**: Microserviço de jogos com Elasticsearch
- **💳 FiapPosTechPayments**: Microserviço de pagamentos
- **🚀 FiapPosTechDocker**: Infraestrutura Docker compartilhada

## 📄 **Documentação Técnica**

- **Swagger API**: `http://localhost:5000/swagger` (durante execução)
- **Arquitetura**: Clean Architecture com CQRS
- **Padrões**: Repository, Unit of Work, AutoMapper

---

**🎆 Microserviço Users com observabilidade completa em produção:**
- **Autenticação JWT** robusta e segura
- **Distributed Tracing** com OpenTelemetry + Jaeger
- **Monitoramento completo** com Prometheus + Grafana + ELK Stack
- **Health checks** em todos os componentes