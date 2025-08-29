@echo off
echo Iniciando Backend Datarisk...

REM Verificar se o Docker está instalado
docker --version >nul 2>&1
if errorlevel 1 (
    echo  Docker nao esta instalado. Por favor, instale o Docker primeiro.
    pause
    exit /b 1
)

REM Verificar se o Docker Compose está instalado
docker-compose --version >nul 2>&1
if errorlevel 1 (
    echo  Docker Compose nao esta instalado. Por favor, instale o Docker Compose primeiro.
    pause
    exit /b 1
)

echo Construindo e iniciando containers...
docker-compose up -d --build

echo Aguardando servicos ficarem prontos...
timeout /t 10 /nobreak >nul

echo Serviços iniciados com sucesso!
echo.
echo  API disponivel em: http://localhost:5000
echo  Documentacao Swagger: http://localhost:5000/swagger
echo  PostgreSQL: localhost:5432
echo.
echo  Para ver os logs: docker-compose logs -f
echo  Para parar: docker-compose down
echo.
echo  Exemplo de uso disponivel em: exemplo-caso-uso.md
pause
