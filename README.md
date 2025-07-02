# AgroTech Safra API

API para gerenciamento de avaliações de pragas em culturas agrícolas.

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- SQL Server (LocalDB ou Express)
- Visual Studio 2022 ou Visual Studio Code

### Passos para Executar

1. **Navegue até a pasta do projeto**
   ```bash
   cd D:\Projetos\AgroTechSafra_Clean
   ```

2. **Restaure os pacotes NuGet**
   ```bash
   dotnet restore
   ```

3. **Configure o banco de dados**
   - Certifique-se de que o SQL Server está rodando
   - A string de conexão está em `appsettings.json`

4. **Execute as migrações (se necessário)**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Execute a API**
   ```bash
   dotnet run
   ```

6. **Acesse o Swagger**
   - Desenvolvimento: `https://localhost:7000/swagger`
   - Ou: `http://localhost:5000/swagger`

## 📋 Endpoints Principais

### Health Check
- `GET /api/health` - Verifica se a API está funcionando
- `GET /api/health/database` - Verifica status do banco

### Avaliações de Pragas
- `GET /api/avaliacoespragas` - Lista todas as avaliações
- `GET /api/avaliacoespragas/{id}` - Busca avaliação por ID
- `POST /api/avaliacoespragas` - Cria nova avaliação
- `PUT /api/avaliacoespragas/{id}` - Atualiza avaliação
- `DELETE /api/avaliacoespragas/{id}` - Remove avaliação

### Amostragem
- `GET /api/amostragem/estatisticas` - Estatísticas gerais
- `GET /api/amostragem/cultura/{cultura}` - Filtra por cultura

## 🏗️ Estrutura do Projeto

```
AgroTechSafra_Clean/
├── Controllers/           # Controllers da API
├── Data/                 # Context do Entity Framework
├── Models/               # Modelos de dados
├── Properties/           # Configurações de launch
├── Program.cs            # Configuração da aplicação
├── appsettings.json      # Configurações
└── AgroTechSafra.API.csproj
```

## 🔧 Problemas Resolvidos

- ✅ Estrutura de solução corrigida
- ✅ Dependências de projetos removidas
- ✅ Entidades integradas ao projeto
- ✅ DbContext configurado
- ✅ Controllers funcionais
- ✅ Swagger configurado

## 📦 Pacotes Utilizados

- Microsoft.EntityFrameworkCore (9.0.3)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.3)
- Microsoft.EntityFrameworkCore.Tools (9.0.3)
- Swashbuckle.AspNetCore (6.6.2)
- CsvHelper (33.1.0)

## 🔍 Testando a API

1. Execute a API
2. Acesse `/api/health` para verificar se está funcionando
3. Use o Swagger para testar os endpoints
4. Crie uma avaliação via POST em `/api/avaliacoespragas`