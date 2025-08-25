
-- =====================================================
-- 1. CRIAR NOVAS TABELAS
-- =====================================================

-- Tabela Scripts
CREATE TABLE IF NOT EXISTS "Scripts" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(500),
    "Codigo" TEXT NOT NULL,
    "CriadoEm" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "AtualizadoEm" TIMESTAMP WITH TIME ZONE
);

-- Tabela Processamentos (Processings)
CREATE TABLE IF NOT EXISTS "Processamentos" (
    "Id" SERIAL PRIMARY KEY,
    "ScriptId" INTEGER NOT NULL,
    "DadosEntrada" TEXT NOT NULL,
    "DadosSaida" TEXT,
    "MensagemErro" TEXT,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Pendente',
    "CriadoEm" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "IniciadoEm" TIMESTAMP WITH TIME ZONE,
    "ConcluidoEm" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Processamentos_Scripts" FOREIGN KEY ("ScriptId") REFERENCES "Scripts"("Id") ON DELETE CASCADE
);

-- Tabela ExecucoesScript (ScriptExecutions)
CREATE TABLE IF NOT EXISTS "ExecucoesScript" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(500),
    "CodigoScript" TEXT NOT NULL,
    "DadosTeste" TEXT NOT NULL,
    "ResultadoEsperado" TEXT,
    "ResultadoReal" TEXT,
    "Sucesso" BOOLEAN NOT NULL DEFAULT FALSE,
    "MensagemErro" TEXT,
    "TempoExecucaoMs" DOUBLE PRECISION,
    "CriadoEm" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "ExecutadoEm" TIMESTAMP WITH TIME ZONE,
    "Categoria" VARCHAR(50) NOT NULL,
    "Versao" INTEGER NOT NULL DEFAULT 1,
    "Ativo" BOOLEAN NOT NULL DEFAULT TRUE,
    "ProcessamentoId" INTEGER,
    CONSTRAINT "FK_ExecucoesScript_Processamentos" FOREIGN KEY ("ProcessamentoId") REFERENCES "Processamentos"("Id") ON DELETE SET NULL
);

