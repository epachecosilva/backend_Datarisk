# Exemplo Prático - Caso de Uso Banco Central

Este documento demonstra como usar a API para processar dados de cartões de crédito do Banco Central do Brasil.

## 1. Hospedar Script de Pré-processamento

Primeiro, vamos hospedar o script JavaScript que processa os dados:

```bash
curl -X POST "http://localhost:5000/api/scripts" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "processamento-cartoes-empresariais",
    "descricao": "Processa dados de cartões empresariais do Bacen - filtra apenas cartões empresariais e agrupa por trimestre e bandeira",
    "codigo": "function process(data) { const corporativeData = data.filter(item => item.produto === \"Empresarial\"); const byQuarterAndIssuer = corporativeData.reduce((acc, item) => { const key = `${item.trimestre}-${item.nomeBandeira}`; if (!acc[key]) { acc[key] = { trimestre: item.trimestre, nomeBandeira: item.nomeBandeira, qtdCartoesEmitidos: 0, qtdCartoesAtivos: 0, qtdTransacoesNacionais: 0, valorTransacoesNacionais: 0, }; } acc[key].qtdCartoesEmitidos += item.qtdCartoesEmitidos; acc[key].qtdCartoesAtivos += item.qtdCartoesAtivos; acc[key].qtdTransacoesNacionais += item.qtdTransacoesNacionais; acc[key].valorTransacoesNacionais += item.valorTransacoesNacionais; return acc; }, {}); return Object.values(byQuarterAndIssuer); }"
  }'
```

**Resposta esperada:**
```json
{
  "id": 1,
  "nome": "processamento-cartoes-empresariais",
  "descricao": "Processa dados de cartões empresariais do Bacen - filtra apenas cartões empresariais e agrupa por trimestre e bandeira",
  "codigo": "function process(data) { ... }",
  "criadoEm": "2024-01-15T10:30:00Z",
  "atualizadoEm": null
}
```

## 2. Executar Pré-processamento

Agora vamos enviar os dados do Banco Central para processamento:

```bash
curl -X POST "http://localhost:5000/api/processamentos" \
  -H "Content-Type: application/json" \
  -d '{
    "scriptId": 1,
    "dadosEntrada": [
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

**Resposta esperada:**
```json
{
  "id": 1,
  "scriptId": 1,
  "dadosEntrada": "[...]",
  "dadosSaida": null,
  "mensagemErro": null,
  "status": "Pendente",
  "criadoEm": "2024-01-15T10:35:00Z",
  "iniciadoEm": null,
  "concluidoEm": null,
  "script": {
    "id": 1,
    "nome": "processamento-cartoes-empresariais",
    "descricao": "Processa dados de cartões empresariais do Bacen...",
    "codigo": "function process(data) { ... }",
    "criadoEm": "2024-01-15T10:30:00Z",
    "atualizadoEm": null
  }
}
```

## 3. Consultar Status e Resultado

Vamos verificar o status do processamento:

```bash
curl -X GET "http://localhost:5000/api/processamentos/1"
```

**Resposta esperada (processamento em andamento):**
```json
{
  "id": 1,
  "scriptId": 1,
  "dadosEntrada": "[...]",
  "dadosSaida": null,
  "mensagemErro": null,
  "status": "Executando",
  "criadoEm": "2024-01-15T10:35:00Z",
  "iniciadoEm": "2024-01-15T10:35:05Z",
  "concluidoEm": null,
  "script": {
    "id": 1,
    "nome": "processamento-cartoes-empresariais",
    "descricao": "Processa dados de cartões empresariais do Bacen...",
    "codigo": "function process(data) { ... }",
    "criadoEm": "2024-01-15T10:30:00Z",
    "atualizadoEm": null
  }
}
```

**Resposta esperada (processamento concluído):**
```json
{
  "id": 1,
  "scriptId": 1,
  "dadosEntrada": "[...]",
  "dadosSaida": [
    {
      "trimestre": "20232",
      "nomeBandeira": "VISA",
      "qtdCartoesEmitidos": 3050384,
      "qtdCartoesAtivos": 1716709,
      "qtdTransacoesNacionais": 43984902,
      "valorTransacoesNacionais": 12846611557.78
    },
    {
      "trimestre": "20232",
      "nomeBandeira": "Mastercard",
      "qtdCartoesEmitidos": 2150384,
      "qtdCartoesAtivos": 1216709,
      "qtdTransacoesNacionais": 33984902,
      "valorTransacoesNacionais": 9846611557.78
    }
  ],
  "mensagemErro": null,
  "status": "Concluido",
  "criadoEm": "2024-01-15T10:35:00Z",
  "iniciadoEm": "2024-01-15T10:35:05Z",
  "concluidoEm": "2024-01-15T10:35:10Z",
  "script": {
    "id": 1,
    "nome": "processamento-cartoes-empresariais",
    "descricao": "Processa dados de cartões empresariais do Bacen...",
    "codigo": "function process(data) { ... }",
    "criadoEm": "2024-01-15T10:30:00Z",
    "atualizadoEm": null
  }
}
```

## 4. Análise dos Resultados

O script processou os dados e retornou:

1. **Filtragem:** Apenas cartões com `produto: "Empresarial"` foram processados
2. **Agrupamento:** Dados agrupados por `trimestre` e `nomeBandeira`
3. **Agregação:** Somas dos valores numéricos para cada grupo
4. **Limpeza:** Removidas informações de transações internacionais

### Resultado Final:
- **VISA Empresarial 2023Q2:** 3.050.384 cartões emitidos, 1.716.709 ativos
- **Mastercard Empresarial 2023Q2:** 2.150.384 cartões emitidos, 1.216.709 ativos
- **American Express:** Não apareceu no resultado (produto "Intermediário")

## 5. Consultar Histórico de Processamentos

Para ver todos os processamentos de um script específico:

```bash
curl -X GET "http://localhost:5000/api/processamentos/script/1"
```

## 6. Documentação da API

Acesse a documentação Swagger em: `http://localhost:5000/swagger`
