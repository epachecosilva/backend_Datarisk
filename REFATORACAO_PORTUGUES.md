# 🇧🇷 Plano de Refatoração para Português

## 🎯 **Objetivo**
Traduzir todo o código do projeto para português brasileiro, mantendo a funcionalidade e melhorando a legibilidade para desenvolvedores brasileiros.

## 📋 **Mapeamento Completo de Traduções**

### **1. Entidades (Entities)**
| **Inglês** | **Português** | **Arquivo** |
|------------|---------------|-------------|
| Script | Roteiro | `src/Datarisk.Core/Entities/Roteiro.cs` |
| Processing | Processamento | `src/Datarisk.Core/Entities/Processamento.cs` |
| ScriptExecution | ExecucaoRoteiro | `src/Datarisk.Core/Entities/ExecucaoRoteiro.cs` |

### **2. Propriedades das Entidades**
| **Inglês** | **Português** |
|------------|---------------|
| Name | Nome |
| Description | Descricao |
| Code | Codigo |
| InputData | DadosEntrada |
| OutputData | DadosSaida |
| ErrorMessage | MensagemErro |
| Status | Status |
| CreatedAt | CriadoEm |
| UpdatedAt | AtualizadoEm |
| StartedAt | IniciadoEm |
| CompletedAt | ConcluidoEm |
| ExecutedAt | ExecutadoEm |
| TestData | DadosTeste |
| ExpectedResult | ResultadoEsperado |
| ActualResult | ResultadoReal |
| IsSuccessful | Sucesso |
| ExecutionTimeMs | TempoExecucaoMs |
| Category | Categoria |
| Version | Versao |
| IsActive | Ativo |
| ProcessingId | ProcessamentoId |
| ScriptId | RoteiroId |

### **3. Enums**
| **Inglês** | **Português** |
|------------|---------------|
| ProcessingStatus | StatusProcessamento |
| Pending | Pendente |
| Running | Executando |
| Completed | Concluido |
| Failed | Falhou |

### **4. Interfaces**
| **Inglês** | **Português** |
|------------|---------------|
| IScriptRepository | IRepositorioRoteiro |
| IProcessingRepository | IRepositorioProcessamento |
| IScriptExecutionRepository | IRepositorioExecucaoRoteiro |
| IScriptExecutionService | IServicoExecucaoRoteiro |

### **5. Repositórios**
| **Inglês** | **Português** |
|------------|---------------|
| ScriptRepository | RepositorioRoteiro |
| ProcessingRepository | RepositorioProcessamento |
| ScriptExecutionRepository | RepositorioExecucaoRoteiro |

### **6. Comandos (Commands)**
| **Inglês** | **Português** |
|------------|---------------|
| CreateScriptCommand | CriarRoteiroComando |
| CreateProcessingCommand | CriarProcessamentoComando |
| CreateScriptExecutionCommand | CriarExecucaoRoteiroComando |
| ExecuteScriptTestCommand | ExecutarTesteRoteiroComando |

### **7. Queries**
| **Inglês** | **Português** |
|------------|---------------|
| GetScriptQuery | ObterRoteiroQuery |
| GetAllScriptsQuery | ObterTodosRoteirosQuery |
| GetProcessingQuery | ObterProcessamentoQuery |
| GetAllProcessingsQuery | ObterTodosProcessamentosQuery |
| GetScriptExecutionQuery | ObterExecucaoRoteiroQuery |
| GetAllScriptExecutionsQuery | ObterTodasExecucoesRoteiroQuery |

### **8. Controllers**
| **Inglês** | **Português** |
|------------|---------------|
| ScriptsController | RoteirosController |
| ProcessingsController | ProcessamentosController |
| ScriptExecutionsController | ExecucoesRoteiroController |

### **9. Serviços**
| **Inglês** | **Português** |
|------------|---------------|
| ScriptExecutionService | ServicoExecucaoRoteiro |

### **10. Contexto do Banco**
| **Inglês** | **Português** |
|------------|---------------|
| DatariskDbContext | ContextoDatarisk |
| Scripts | Roteiros |
| Processings | Processamentos |
| ScriptExecutions | ExecucoesRoteiro |

## 🔄 **Ordem de Refatoração**

### **Fase 1: Entidades Core**
1. ✅ Renomear arquivos de entidades
2. ✅ Traduzir propriedades
3. ✅ Atualizar namespaces
4. ✅ Traduzir enums

### **Fase 2: Interfaces**
1. ✅ Renomear interfaces
2. ✅ Atualizar assinaturas de métodos
3. ✅ Traduzir nomes de métodos

### **Fase 3: Repositórios**
1. ✅ Renomear classes de repositório
2. ✅ Atualizar implementações
3. ✅ Traduzir métodos

### **Fase 4: Application Layer**
1. ✅ Renomear comandos
2. ✅ Renomear queries
3. ✅ Atualizar handlers
4. ✅ Traduzir serviços

### **Fase 5: API Layer**
1. ✅ Renomear controllers
2. ✅ Atualizar rotas
3. ✅ Traduzir endpoints
4. ✅ Atualizar requests/responses

### **Fase 6: Infrastructure**
1. ✅ Atualizar DbContext
2. ✅ Renomear tabelas no banco
3. ✅ Atualizar configurações EF

### **Fase 7: Testes**
1. ✅ Renomear classes de teste
2. ✅ Atualizar referências
3. ✅ Traduzir nomes de métodos de teste

## 🛠️ **Scripts de Automação**

### **Script 1: Renomear Arquivos**
```bash
# Renomear entidades
mv src/Datarisk.Core/Entities/Script.cs src/Datarisk.Core/Entities/Roteiro.cs
mv src/Datarisk.Core/Entities/Processing.cs src/Datarisk.Core/Entities/Processamento.cs
mv src/Datarisk.Core/Entities/ScriptExecution.cs src/Datarisk.Core/Entities/ExecucaoRoteiro.cs

# Renomear interfaces
mv src/Datarisk.Core/Interfaces/IScriptRepository.cs src/Datarisk.Core/Interfaces/IRepositorioRoteiro.cs
mv src/Datarisk.Core/Interfaces/IProcessingRepository.cs src/Datarisk.Core/Interfaces/IRepositorioProcessamento.cs
mv src/Datarisk.Core/Interfaces/IScriptExecutionRepository.cs src/Datarisk.Core/Interfaces/IRepositorioExecucaoRoteiro.cs
mv src/Datarisk.Core/Interfaces/IScriptExecutionService.cs src/Datarisk.Core/Interfaces/IServicoExecucaoRoteiro.cs

# Renomear repositórios
mv src/Datarisk.Infrastructure/Repositories/ScriptRepository.cs src/Datarisk.Infrastructure/Repositories/RepositorioRoteiro.cs
mv src/Datarisk.Infrastructure/Repositories/ProcessingRepository.cs src/Datarisk.Infrastructure/Repositories/RepositorioProcessamento.cs
mv src/Datarisk.Infrastructure/Repositories/ScriptExecutionRepository.cs src/Datarisk.Infrastructure/Repositories/RepositorioExecucaoRoteiro.cs

# Renomear controllers
mv src/Datarisk.Api/Controllers/ScriptsController.cs src/Datarisk.Api/Controllers/RoteirosController.cs
mv src/Datarisk.Api/Controllers/ProcessingsController.cs src/Datarisk.Api/Controllers/ProcessamentosController.cs
mv src/Datarisk.Api/Controllers/ScriptExecutionsController.cs src/Datarisk.Api/Controllers/ExecucoesRoteiroController.cs
```

### **Script 2: Substituir Texto**
```bash
# Substituir nomes de classes
find . -name "*.cs" -exec sed -i 's/Script/Roteiro/g' {} \;
find . -name "*.cs" -exec sed -i 's/Processing/Processamento/g' {} \;
find . -name "*.cs" -exec sed -i 's/ScriptExecution/ExecucaoRoteiro/g' {} \;

# Substituir propriedades
find . -name "*.cs" -exec sed -i 's/Name/Nome/g' {} \;
find . -name "*.cs" -exec sed -i 's/Description/Descricao/g' {} \;
find . -name "*.cs" -exec sed -i 's/Code/Codigo/g' {} \;
find . -name "*.cs" -exec sed -i 's/InputData/DadosEntrada/g' {} \;
find . -name "*.cs" -exec sed -i 's/OutputData/DadosSaida/g' {} \;
find . -name "*.cs" -exec sed -i 's/ErrorMessage/MensagemErro/g' {} \;
find . -name "*.cs" -exec sed -i 's/CreatedAt/CriadoEm/g' {} \;
find . -name "*.cs" -exec sed -i 's/UpdatedAt/AtualizadoEm/g' {} \;
find . -name "*.cs" -exec sed -i 's/StartedAt/IniciadoEm/g' {} \;
find . -name "*.cs" -exec sed -i 's/CompletedAt/ConcluidoEm/g' {} \;
find . -name "*.cs" -exec sed -i 's/ExecutedAt/ExecutadoEm/g' {} \;
find . -name "*.cs" -exec sed -i 's/TestData/DadosTeste/g' {} \;
find . -name "*.cs" -exec sed -i 's/ExpectedResult/ResultadoEsperado/g' {} \;
find . -name "*.cs" -exec sed -i 's/ActualResult/ResultadoReal/g' {} \;
find . -name "*.cs" -exec sed -i 's/IsSuccessful/Sucesso/g' {} \;
find . -name "*.cs" -exec sed -i 's/ExecutionTimeMs/TempoExecucaoMs/g' {} \;
find . -name "*.cs" -exec sed -i 's/Category/Categoria/g' {} \;
find . -name "*.cs" -exec sed -i 's/Version/Versao/g' {} \;
find . -name "*.cs" -exec sed -i 's/IsActive/Ativo/g' {} \;
```

## 📊 **Impacto da Refatoração**

### **✅ Vantagens:**
- 🎯 **Legibilidade**: Código mais fácil de entender para brasileiros
- 🏢 **Padrão Empresarial**: Alinhado com padrões brasileiros
- 📚 **Documentação**: Comentários e nomes em português
- 👥 **Equipe**: Facilita onboarding de novos desenvolvedores

### **⚠️ Considerações:**
- 🔄 **Migração**: Requer atualização do banco de dados
- 🔗 **Dependências**: Pode afetar integrações externas
- 📝 **Documentação**: Necessário atualizar toda documentação
- 🧪 **Testes**: Todos os testes precisam ser atualizados

## 🚀 **Implementação**

### **Opção 1: Refatoração Automática (Recomendado)**
- Usar scripts de automação
- Executar em ambiente de desenvolvimento
- Testar extensivamente antes de produção

### **Opção 2: Refatoração Manual**
- Fazer mudanças arquivo por arquivo
- Mais seguro, mas demorado
- Melhor controle sobre cada mudança

### **Opção 3: Refatoração Gradual**
- Fazer mudanças por camada
- Testar cada camada antes de prosseguir
- Manter compatibilidade durante transição

## 📋 **Checklist de Validação**

- [ ] ✅ Todos os arquivos renomeados
- [ ] ✅ Todas as classes traduzidas
- [ ] ✅ Todas as propriedades traduzidas
- [ ] ✅ Todos os métodos traduzidos
- [ ] ✅ Namespaces atualizados
- [ ] ✅ Referências corrigidas
- [ ] ✅ Banco de dados migrado
- [ ] ✅ Testes atualizados
- [ ] ✅ Documentação atualizada
- [ ] ✅ API endpoints funcionando
- [ ] ✅ Swagger atualizado

---

**Status**: 📋 **PLANO CRIADO**  
**Próximo Passo**: Implementar a refatoração  
**Tempo Estimado**: 2-3 horas  
**Risco**: Baixo (com backup e testes)
