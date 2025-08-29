#!/bin/bash

echo " Iniciando Backend Datarisk..."

# Verificar se o Docker está instalado
if ! command -v docker &> /dev/null; then
    echo " Docker nao esta instalado. Por favor, instale o Docker primeiro."
    exit 1
fi

# Verificar se o Docker Compose está instalado
if ! command -v docker-compose &> /dev/null; then
    echo " Docker Compose nao esta instalado. Por favor, instale o Docker Compose primeiro."
    exit 1
fi

echo " Construindo e iniciando containers..."
docker-compose up -d --build

echo " Aguardando servicos ficarem prontos..."
sleep 10

echo " Serviços iniciados com sucesso!"
echo ""
echo " API disponível em: http://localhost:5000"
echo " Documentacao Swagger: http://localhost:5000/swagger"
echo " PostgreSQL: localhost:5432"
echo ""
echo " Para ver os logs: docker-compose logs -f"
echo " Para parar: docker-compose down"
echo ""
echo " Exemplo de uso disponivel em: exemplo-caso-uso.md"
