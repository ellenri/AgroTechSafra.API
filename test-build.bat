@echo off
echo ========================================
echo  Testando compilacao da AgroTech Safra API
echo ========================================
echo.

cd /d "D:\Projetos\AgroTechSafra_Clean"

echo [1/4] Restaurando pacotes NuGet...
dotnet restore

echo.
echo [2/4] Compilando o projeto...
dotnet build

echo.
echo [3/4] Verificando se a compilacao foi bem-sucedida...
if %ERRORLEVEL% EQU 0 (
    echo ✅ Compilacao bem-sucedida!
    echo.
    echo [4/4] Para executar a API, use:
    echo    dotnet run
    echo.
    echo Acesse: https://localhost:7000/swagger
) else (
    echo ❌ Erro na compilacao!
    echo Verifique os erros acima.
)

echo.
pause