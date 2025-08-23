# 🚀 Demonstração do Fluxo Completo de Pré-processamento

Este guia demonstra o **fluxo completo** de processamento de scripts, desde a criação até a execução e versionamento.

## 📋 Índice

1. [Estrutura de Dados Criada](#estrutura-de-dados-criada)
2. [Scripts de Teste Realistas](#scripts-de-teste-realistas)
3. [Fluxo de Execução](#fluxo-de-execução)
4. [Demonstração Prática](#demonstração-prática)
5. [Versionamento de Scripts](#versionamento-de-scripts)
6. [Monitoramento e Resultados](#monitoramento-e-resultados)

---

## 🗄️ Estrutura de Dados Criada

### Tabelas Principais:

#### 1. **Scripts** (Scripts hospedados)
```sql
- Id (PK)
- Name (Nome do script)
- Description (Descrição)
- Code (Código JavaScript)
- CreatedAt (Data de criação)
- UpdatedAt (Data de atualização)
```

#### 2. **Processings** (Execuções de processamento)
```sql
- Id (PK)
- ScriptId (FK para Scripts)
- InputData (Dados de entrada JSON)
- OutputData (Dados de saída JSON)
- Status (Pending, Running, Completed, Failed)
- ErrorMessage (Mensagem de erro)
- CreatedAt, StartedAt, CompletedAt (Timestamps)
```

#### 3. **ScriptExecutions** (Versionamento de testes) ⭐ **NOVO**
```sql
- Id (PK)
- Name (Nome do teste)
- Description (Descrição)
- ScriptCode (Código do script)
- TestData (Dados de teste JSON)
- ExpectedResult (Resultado esperado)
- ActualResult (Resultado real)
- IsSuccessful (Sucesso/Falha)
- ErrorMessage (Mensagem de erro)
- ExecutionTimeMs (Tempo de execução)
- Category (Categoria: Banco Central, E-commerce, etc.)
- Version (Versão do script)
- IsActive (Ativo/Inativo)
- ProcessingId (FK opcional para Processings)
```

---

## 📜 Scripts de Teste Realistas

### 1. **Banco Central - Estatísticas de Pagamento**
**Arquivo:** `test-scripts/banco-central-payment-stats.js`

**Funcionalidade:**
- Filtra apenas cartões empresariais
- Agrupa por trimestre e bandeira
- Soma transações nacionais e internacionais
- Remove dados de cartões não-empresariais

**Dados de Teste:** `test-data/banco-central-payment-data.json`
- 9 registros com dados realistas do Bacen
- Múltiplas bandeiras (VISA, Mastercard, American Express)
- Dados de 3 trimestres (20231, 20232, 20233)

### 2. **E-commerce - Análise de Vendas**
**Arquivo:** `test-scripts/ecommerce-sales-analysis.js`

**Funcionalidade:**
- Filtra vendas dos últimos 6 meses
- Agrupa por categoria
- Calcula métricas (ticket médio, produtos mais vendidos)
- Analisa vendas por mês

**Dados de Teste:** `test-data/ecommerce-sales-data.json`
- 15 transações de vendas
- 3 categorias (Eletrônicos, Esportes, Livros)
- Dados de janeiro a março de 2024

### 3. **Customer Segmentation**
**Arquivo:** `test-scripts/customer-segmentation.js`

**Funcionalidade:**
- Analisa comportamento de compra
- Cria segmentos (Bronze, Prata, Ouro, Diamante)
- Calcula métricas RFM (Recência, Frequência, Valor)
- Identifica categorias preferidas

**Dados de Teste:** `test-data/customer-purchase-data.json`
- 15 compras de 10 clientes diferentes
- Dados de comportamento de compra
- Múltiplas categorias e valores

---

## 🔄 Fluxo de Execução

### **Etapa 1: Criação do Script**
```bash
POST /api/scripts
{
  "name": "Banco Central - Estatísticas",
  "description": "Processa dados do Bacen",
  "code": "function process(data) { ... }"
}
```

### **Etapa 2: Criação do Teste**
```bash
POST /api/scriptexecutions
{
  "name": "Teste Banco Central v1",
  "description": "Teste com dados reais",
  "scriptCode": "function process(data) { ... }",
  "testData": "[{...}]",
  "category": "Banco Central"
}
```

### **Etapa 3: Execução do Teste**
```bash
POST /api/scriptexecutions/{id}/execute
```

### **Etapa 4: Processamento Real**
```bash
POST /api/processings
{
  "scriptId": 1,
  "inputData": "[{...}]"
}
```

### **Etapa 5: Monitoramento**
```bash
GET /api/processings/{id}
GET /api/scriptexecutions/{id}
```

---

## 🧪 Demonstração Prática

### **Passo 1: Iniciar a Aplicação**
```bash
# Opção A: Docker (Recomendado)
docker-compose up -d

# Opção B: Visual Studio 2022
# Abrir Datarisk.sln e pressionar F5
```

### **Passo 2: Popular Dados de Teste**
```bash
# Executar o script SQL
psql -h localhost -U postgres -d datarisk -f scripts/populate-test-data.sql
```

### **Passo 3: Testar via Swagger**
1. Acessar: `http://localhost:5000/swagger`
2. Testar os endpoints na ordem:

#### **3.1 Criar Script**
```json
POST /api/scripts
{
  "name": "Meu Script de Teste",
  "description": "Script personalizado",
  "code": "function process(data) { return data.filter(item => item.ativo === true); }"
}
```

#### **3.2 Criar Execução de Teste**
```json
POST /api/scriptexecutions
{
  "name": "Teste Personalizado v1",
  "description": "Teste do meu script",
  "scriptCode": "function process(data) { return data.filter(item => item.ativo === true); }",
  "testData": "[{\"id\": 1, \"ativo\": true}, {\"id\": 2, \"ativo\": false}]",
  "category": "Teste"
}
```

#### **3.3 Executar o Teste**
```bash
POST /api/scriptexecutions/{id}/execute
```

#### **3.4 Verificar Resultado**
```bash
GET /api/scriptexecutions/{id}
```

### **Passo 4: Testar com Dados Reais**

#### **4.1 Banco Central**
```bash
# Criar script
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d @test-scripts/banco-central-payment-stats.js

# Executar processamento
curl -X POST "http://localhost:5000/api/processings" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 1,
    "inputData": '"$(cat test-data/banco-central-payment-data.json)"'
  }'
```

#### **4.2 E-commerce**
```bash
# Criar script
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d @test-scripts/ecommerce-sales-analysis.js

# Executar processamento
curl -X POST "http://localhost:5000/api/processings" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 2,
    "inputData": '"$(cat test-data/ecommerce-sales-data.json)"'
  }'
```

---

## 🔄 Versionamento de Scripts

### **Conceito:**
- Cada script pode ter múltiplas versões
- Versões são incrementadas automaticamente
- Histórico completo de mudanças
- Possibilidade de ativar/desativar versões

### **Exemplo de Versionamento:**

#### **Versão 1.0 - Script Básico**
```javascript
function process(data) {
  return data.filter(item => item.ativo === true);
}
```

#### **Versão 2.0 - Script Melhorado**
```javascript
function process(data) {
  return data
    .filter(item => item.ativo === true)
    .map(item => ({
      ...item,
      processadoEm: new Date().toISOString()
    }));
}
```

#### **Versão 3.0 - Script com Validação**
```javascript
function process(data) {
  if (!Array.isArray(data)) {
    throw new Error('Dados devem ser um array');
  }
  
  return data
    .filter(item => item.ativo === true)
    .map(item => ({
      ...item,
      processadoEm: new Date().toISOString(),
      validado: true
    }));
}
```

### **API de Versionamento:**
```bash
# Listar todas as versões de um script
GET /api/scriptexecutions?name=MeuScript

# Executar versão específica
POST /api/scriptexecutions/{id}/execute

# Desativar versão
PUT /api/scriptexecutions/{id}
{
  "isActive": false
}
```

---

## 📊 Monitoramento e Resultados

### **Métricas Coletadas:**

#### **1. Performance**
- Tempo de execução (ms)
- Status de sucesso/falha
- Mensagens de erro detalhadas

#### **2. Versionamento**
- Histórico de versões
- Comparação entre versões
- Taxa de sucesso por versão

#### **3. Categorização**
- Scripts por categoria
- Análise de uso por domínio
- Tendências de desenvolvimento

### **Exemplo de Resultado:**

#### **Script Banco Central - Sucesso**
```json
{
  "id": 1,
  "name": "Teste Banco Central v1",
  "category": "Banco Central",
  "version": 1,
  "isSuccessful": true,
  "executionTimeMs": 45.2,
  "actualResult": [
    {
      "trimestre": "20231",
      "nomeBandeira": "VISA",
      "qtdCartoesEmitidos": 3050384,
      "qtdCartoesAtivos": 1716709,
      "qtdTransacoesNacionais": 43984902,
      "valorTransacoesNacionais": 12846611557.78
    }
  ],
  "executedAt": "2024-01-15T10:30:00Z"
}
```

#### **Script com Erro**
```json
{
  "id": 2,
  "name": "Teste com Erro v1",
  "category": "Teste",
  "version": 1,
  "isSuccessful": false,
  "executionTimeMs": 12.5,
  "errorMessage": "Script execution failed: Property 'filter' of object is not a function",
  "executedAt": "2024-01-15T10:35:00Z"
}
```

---

## 🎯 Benefícios da Implementação

### **1. Rastreabilidade Completa**
- Histórico de todas as execuções
- Versionamento automático
- Comparação entre versões

### **2. Qualidade de Dados**
- Testes automatizados
- Validação de scripts
- Dados de teste realistas

### **3. Monitoramento em Tempo Real**
- Status de execução
- Métricas de performance
- Alertas de erro

### **4. Flexibilidade**
- Múltiplas categorias
- Scripts personalizados
- Dados de teste variados

### **5. Escalabilidade**
- Execução assíncrona
- Sandboxing seguro
- Versionamento robusto

---

## 🚀 Próximos Passos

1. **Executar a demonstração completa**
2. **Testar com seus próprios scripts**
3. **Analisar os resultados no banco**
4. **Explorar o versionamento**
5. **Criar novos casos de uso**

---

**🎉 Parabéns!** Você agora tem um ambiente completo de MLOps para pré-processamento de dados com scripts realistas, versionamento e monitoramento completo!
