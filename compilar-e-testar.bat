@echo off
echo ========================================
echo  COMPILANDO AGROTECHSAFRA API COM IA
echo ========================================
echo.

cd /d "D:\Projetos\AgroTechSafra.API"

echo [1/4] Restaurando pacotes NuGet...
dotnet restore

echo.
echo [2/4] Limpando compilação anterior...
dotnet clean

echo.
echo [3/4] Compilando projeto...
dotnet build --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ COMPILAÇÃO BEM-SUCEDIDA!
    echo.
    echo 🤖 FUNCIONALIDADES ADICIONADAS:
    echo    - Integração com OpenAI configurada
    echo    - Análise inteligente de pragas
    echo    - Modelos estruturados para IA
    echo    - Endpoints especializados
    echo.
    echo 🚀 PARA EXECUTAR:
    echo    dotnet run
    echo.
    echo 🌐 ACESSE:
    echo    https://localhost:7000 ou http://localhost:5000
    echo.
    echo 📚 ENDPOINTS PRINCIPAIS:
    echo    POST /api/analiseamostragem/analisar - Análise principal
    echo    GET  /api/analiseamostragem/testar-ia - Testar OpenAI
    echo    POST /api/analiseamostragem/processar-csv - Debug CSV
    echo.
    echo 💡 TESTE COM ARQUIVO:
    echo    amostragem_pragas.csv (já disponível na pasta)
) else (
    echo.
    echo ❌ ERRO NA COMPILAÇÃO!
    echo Verifique os erros acima e corrija antes de continuar.
    echo.
    echo 🔧 POSSÍVEIS SOLUÇÕES:
    echo    1. Verificar se .NET 8 está instalado
    echo    2. Executar: dotnet restore
    echo    3. Verificar conectividade para baixar pacotes
)

echo.
pause