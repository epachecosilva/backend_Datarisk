# Como Executar no Visual Studio 2022

## 🚀 Opção A: Docker (Recomendado)

### 1. Instalar Docker Desktop
- Baixe e instale o [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Inicie o Docker Desktop

### 2. Executar com Docker Compose
```bash
# No terminal, na pasta do projeto
docker-compose up -d
```

### 3. Abrir no Visual Studio
- Abra o Visual Studio 2022
- Abra a solução `Datarisk.sln`
- Configure `Datarisk.Api` como projeto de inicialização
- Pressione F5 ou clique em "Start"

### 4. Verificar se Funcionou
- A API estará disponível em: `http://localhost:5000`
- Swagger em: `http://localhost:5000/swagger`
- Banco PostgreSQL em: `localhost:5432`

---

## 🖥️ Opção B: Local (PostgreSQL Instalado)

### 1. Instalar PostgreSQL
- Baixe e instale o [PostgreSQL](https://www.postgresql.org/download/)
- Use as credenciais padrão: `postgres/postgres`

### 2. Criar Banco de Dados
```sql
CREATE DATABASE datarisk;
```

### 3. Configurar Connection String
No arquivo `src/Datarisk.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=datarisk;Username=postgres;Password=postgres"
  }
}
```

### 4. Executar Migrações
```bash
# No Package Manager Console do Visual Studio
Update-Database -ProjectName Datarisk.Infrastructure -StartupProjectName Datarisk.Api
```

### 5. Executar o Projeto
- Pressione F5 ou clique em "Start"

---

## 🧪 Testando a API

### 1. Acessar Swagger
- Vá para: `http://localhost:5000/swagger`
- Teste os endpoints diretamente na interface

### 2. Testar com Postman/curl

#### Criar Script:
```bash
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "teste-filtro",
    "description": "Script de teste",
    "code": "function process(data) { return data.filter(item => item.ativo === true); }"
  }'
```

#### Executar Processamento:
```bash
curl -X POST "http://localhost:5000/api/processings" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 1,
    "inputData": "[{\"id\": 1, \"ativo\": true}, {\"id\": 2, \"ativo\": false}]"
  }'
```

#### Verificar Status:
```bash
curl -X GET "http://localhost:5000/api/processings/1"
```

---

## 🔍 Verificando se Deu Certo

### ✅ Sinais de Sucesso:
1. **Visual Studio compila sem erros**
2. **API inicia sem erros no console**
3. **Swagger abre em `http://localhost:5000/swagger`**
4. **Banco de dados conecta (sem erros de conexão)**
5. **Testes passam (Test Explorer)**

### ❌ Possíveis Problemas:
1. **Erro de conexão com banco** → Verificar PostgreSQL/Docker
2. **Erro de compilação** → Restaurar pacotes NuGet
3. **Porta em uso** → Mudar porta no `launchSettings.json`
4. **Dependências faltando** → `dotnet restore`

---

## 🛠️ Troubleshooting

### Restaurar Pacotes NuGet:
```bash
dotnet restore
```

### Limpar e Rebuild:
```bash
dotnet clean
dotnet build
```

### Verificar Logs:
- No Visual Studio: Output Window
- No console da aplicação
- Logs do Docker: `docker-compose logs`

### Executar Testes:
```bash
dotnet test
```
