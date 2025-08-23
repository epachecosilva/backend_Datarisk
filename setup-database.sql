-- Script para configurar o banco de dados PostgreSQL
-- Execute este script no pgAdmin ou psql

-- Criar banco de dados (se não existir)
CREATE DATABASE datarisk;

-- Conectar ao banco datarisk
\c datarisk;

-- Criar usuário (opcional - se quiser usar um usuário específico)
-- CREATE USER datarisk_user WITH PASSWORD 'datarisk_password';
-- GRANT ALL PRIVILEGES ON DATABASE datarisk TO datarisk_user;

-- Verificar se as tabelas foram criadas (serão criadas automaticamente pelo Entity Framework)
-- SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';
