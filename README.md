# Backend Datarisk - API de Pré-processamento de Dados

Esta é uma implementação do desafio prático para backend da Datarisk, desenvolvida em .NET Core com C#.

## Descrição

A API oferece serviços de MLOps para pré-processamento de dados, permitindo:
- Hospedar scripts JavaScript de pré-processamento
- Executar transformações de dados de forma assíncrona
- Consultar resultados e status das execuções
- Persistir dados em banco PostgreSQL

## Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **Entity Framework Core** - ORM para PostgreSQL
- **Docker** - Containerização
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Testes automatizados
- **MediatR** - Padrão CQRS para organização do código

## Estrutura do Projeto

```
backend_Datarisk/
├── src/
│   ├── Datarisk.Api/              # API principal
│   ├── Datarisk.Core/             # Entidades e interfaces
│   ├── Datarisk.Infrastructure/   # Implementações de repositório e banco
│   └── Datarisk.Application/      # Casos de uso e serviços
├── tests/
│   └── Datarisk.Tests/            # Testes automatizados
├── docker-compose.yml             # Orquestração de containers
└── Dockerfile                     # Container da API
```

## Como Executar

### Pré-requisitos
- Docker e Docker Compose
- .NET 8.0 SDK (para desenvolvimento local)

### Execução com Docker
```bash
# Clone o repositório
git clone <repository-url>
cd backend_Datarisk

# Execute com Docker Compose
docker-compose up -d
```

A API estará disponível em: `http://localhost:5000`
Documentação Swagger: `http://localhost:5000/swagger`

### Execução Local
```bash
# Restaurar dependências
dotnet restore

# Executar migrações
dotnet ef database update --project src/Datarisk.Infrastructure --startup-project src/Datarisk.Api

# Executar a API
dotnet run --project src/Datarisk.Api
```

## Caso de Uso - Dados do Banco Central

### 1. Hospedar Script de Pré-processamento

```bash
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "processamento-cartoes-empresariais",
    "description": "Processa dados de cartões empresariais do Bacen",
    "code": "function process(data) { const corporativeData = data.filter(item => item.produto === \"Empresarial\"); const byQuarterAndIssuer = corporativeData.reduce((acc, item) => { const key = `${item.trimestre}-${item.nomeBandeira}`; if (!acc[key]) { acc[key] = { trimestre: item.trimestre, nomeBandeira: item.nomeBandeira, qtdCartoesEmitidos: 0, qtdCartoesAtivos: 0, qtdTransacoesNacionais: 0, valorTransacoesNacionais: 0, }; } acc[key].qtdCartoesEmitidos += item.qtdCartoesEmitidos; acc[key].qtdCartoesAtivos += item.qtdCartoesAtivos; acc[key].qtdTransacoesNacionais += item.qtdTransacoesNacionais; acc[key].valorTransacoesNacionais += item.valorTransacoesNacionais; return acc; }, {}); return Object.values(byQuarterAndIssuer); }"
  }'
```

### 2. Executar Pré-processamento

```bash
curl -X POST "http://localhost:5000/api/processings" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": "1",
    "data": [
      {
        "trimestre": "20231",
        "nomeBandeira": "American Express",
        "nomeFuncao": "Crédito",
        "produto": "Intermediário",
        "qtdCartoesEmitidos": 433549,
        "qtdCartoesAtivos": 335542,
        "qtdTransacoesNacionais": 9107357,
        "valorTransacoesNacionais": 1617984610.42,
        "qtdTransacoesInternacionais": 76424,
        "valorTransacoesInternacionais": 41466368.94
      },
      {
        "trimestre": "20232",
        "nomeBandeira": "VISA",
        "nomeFuncao": "Crédito",
        "produto": "Empresarial",
        "qtdCartoesEmitidos": 3050384,
        "qtdCartoesAtivos": 1716709,
        "qtdTransacoesNacionais": 43984902,
        "valorTransacoesNacionais": 12846611557.78,
        "qtdTransacoesInternacionais": 470796,
        "valorTransacoesInternacionais": 397043258.04
      }
    ]
  }'
```

### 3. Consultar Status e Resultado

```bash
curl -X GET "http://localhost:5000/api/processings/{processingId}"
```

## Endpoints da API

### Scripts
- `POST /api/scripts` - Criar novo script
- `GET /api/scripts` - Listar scripts
- `GET /api/scripts/{id}` - Obter script por ID
- `PUT /api/scripts/{id}` - Atualizar script
- `DELETE /api/scripts/{id}` - Remover script

### Processamentos
- `POST /api/processings` - Iniciar processamento
- `GET /api/processings` - Listar processamentos
- `GET /api/processings/{id}` - Obter status e resultado

## Características Implementadas

✅ **Requisitos Obrigatórios:**
- API REST com JSON
- Persistência em PostgreSQL
- Execução assíncrona de scripts
- Ambiente seguro para execução
- Identificação de scripts e processamentos
- Consulta de status e resultados

✅ **Extras Implementados:**
- Documentação OpenAPI/Swagger
- Validação de scripts JavaScript
- Testes automatizados
- Containerização com Docker
- Respostas ao questionário técnico

## Segurança

- Scripts são executados em ambiente isolado
- Validação de sintaxe JavaScript
- Sanitização de entrada
- Logs de auditoria

## Testes

```bash
# Executar testes
dotnet test tests/Datarisk.Tests/
```

## Respostas ao Questionário

As respostas detalhadas ao questionário técnico estão disponíveis no arquivo `QUESTIONARIO.md`.
