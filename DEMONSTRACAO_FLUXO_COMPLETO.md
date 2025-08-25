# Demonstração do Fluxo Completo de Pré-processamento

Este guia demonstra o **fluxo completo** de processamento de scripts, desde a criação até a execução e versionamento.

##  Índice

1. [Estrutura de Dados Criada](#estrutura-de-dados-criada)
2. [Scripts de Teste Realistas](#scripts-de-teste-realistas)
3. [Fluxo de Execução](#fluxo-de-execução)
4. [Demonstração Prática](#demonstração-prática)
5. [Versionamento de Scripts](#versionamento-de-scripts)
6. [Monitoramento e Resultados](#monitoramento-e-resultados)

---

##  Estrutura de Dados Criada

### Tabelas Principais:

#### 1. **Scripts** (Scripts hospedados)
```sql
- Id (PK)
- Nome (Nome do script)
- Descricao (Descrição)
- Codigo (Código JavaScript)
- CriadoEm (Data de criação)
- AtualizadoEm (Data de atualização)
```

#### 2. **Processamentos** (Execuções de processamento)
```sql
- Id (PK)
- ScriptId (FK para Scripts)
- DadosEntrada (Dados de entrada JSON)
- DadosSaida (Dados de saída JSON)
- Status (Pendente, Executando, Concluido, Falhou)
- MensagemErro (Mensagem de erro)
- CriadoEm, IniciadoEm, ConcluidoEm (Timestamps)
```

#### 3. **ExecucoesScript** (Versionamento de testes) ⭐ **NOVO**
```sql
- Id (PK)
- Nome (Nome do teste)
- Descricao (Descrição)
- CodigoScript (Código do script)
- DadosTeste (Dados de teste JSON)
- ResultadoEsperado (Resultado esperado)
- ResultadoReal (Resultado real)
- Sucesso (Sucesso/Falha)
- MensagemErro (Mensagem de erro)
- TempoExecucaoMs (Tempo de execução)
- Categoria (Categoria: Banco Central, E-commerce, etc.)
- Versao (Versão do script)
- Ativo (Ativo/Inativo)
- ProcessamentoId (FK opcional para Processamentos)
```

---

## Scripts de Teste Realistas

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

## Fluxo de Execução

### **Etapa 1: Criação do Script**
```bash
POST /api/scripts
{
  "nome": "Banco Central - Estatísticas",
  "descricao": "Processa dados do Bacen",
  "codigo": "function process(data) { ... }"
}
```

### **Etapa 2: Criação do Teste**
```bash
POST /api/execucoesScript
{
  "nome": "Teste Banco Central v1",
  "descricao": "Teste com dados reais",
  "codigoScript": "function process(data) { ... }",
  "dadosTeste": "[{...}]",
  "categoria": "Banco Central"
}
```

### **Etapa 3: Execução do Teste**
```bash
POST /api/execucoesScript/{id}/executar
```

### **Etapa 4: Processamento Real**
```bash
POST /api/processamentos
{
  "scriptId": 1,
  "dadosEntrada": "[{...}]"
}
```

### **Etapa 5: Monitoramento**
```bash
GET /api/processamentos/{id}
GET /api/execucoesScript/{id}
```

---

##  Demonstração Prática

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
  "nome": "Meu Script de Teste",
  "descricao": "Script personalizado",
  "codigo": "function process(data) { return data.filter(item => item.ativo === true); }"
}
```

#### **3.2 Criar Execução de Teste**
```json
POST /api/execucoesScript
{
  "nome": "Teste Personalizado v1",
  "descricao": "Teste do meu script",
  "codigoScript": "function process(data) { return data.filter(item => item.ativo === true); }",
  "dadosTeste": "[{\"id\": 1, \"ativo\": true}, {\"id\": 2, \"ativo\": false}]",
  "categoria": "Teste"
}
```

#### **3.3 Executar o Teste**
```bash
POST /api/execucoesScript/{id}/executar
```

#### **3.4 Verificar Resultado**
```bash
GET /api/execucoesScript/{id}
```

### **Passo 4: Testar com Dados Reais**

#### **4.1 Banco Central**
```bash
# Criar script
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d @test-scripts/banco-central-payment-stats.js

# Executar processamento
curl -X POST "http://localhost:5000/api/processamentos" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 1,
    "dadosEntrada": '"$(cat test-data/banco-central-payment-data.json)"'
  }'
```

#### **4.2 E-commerce**
```bash
# Criar script
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d @test-scripts/ecommerce-sales-analysis.js

# Executar processamento
curl -X POST "http://localhost:5000/api/processamentos" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 2,
    "dadosEntrada": '"$(cat test-data/ecommerce-sales-data.json)"'
  }'
```

---

## Versionamento de Scripts

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
GET /api/execucoesScript?nome=MeuScript

# Executar versão específica
POST /api/execucoesScript/{id}/executar

# Desativar versão
PUT /api/execucoesScript/{id}
{
  "ativo": false
}
```

---

## Monitoramento e Resultados

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
  "nome": "Teste Banco Central v1",
  "categoria": "Banco Central",
  "versao": 1,
  "sucesso": true,
  "tempoExecucaoMs": 45.2,
  "resultadoReal": [
    {
      "trimestre": "20231",
      "nomeBandeira": "VISA",
      "qtdCartoesEmitidos": 3050384,
      "qtdCartoesAtivos": 1716709,
      "qtdTransacoesNacionais": 43984902,
      "valorTransacoesNacionais": 12846611557.78
    }
  ],
  "executadoEm": "2024-01-15T10:30:00Z"
}
```

#### **Script com Erro**
```json
{
  "id": 2,
  "nome": "Teste com Erro v1",
  "categoria": "Teste",
  "versao": 1,
  "sucesso": false,
  "tempoExecucaoMs": 12.5,
  "mensagemErro": "Falha na execução do script: Property 'filter' of object is not a function",
  "executadoEm": "2024-01-15T10:35:00Z"
}
```
