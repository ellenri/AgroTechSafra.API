# 🤖 AgroTech Safra API - Integração com IA

API atualizada com análise inteligente de pragas agrícolas usando OpenAI.

## 🚀 COMO EXECUTAR

### 1. **Restaurar Pacotes**
```bash
cd D:\Projetos\AgroTechSafra.API
dotnet restore
```

### 2. **Executar API**
```bash
dotnet run
```

### 3. **Acessar Swagger**
- `https://localhost:7000` ou conforme configurado
- Interface completa para testar

---

## 🔧 MODIFICAÇÕES IMPLEMENTADAS

### ✅ **PROBLEMAS CORRIGIDOS:**

1. **JSON inválido no appsettings.json** - Corrigido
2. **Pacote OpenAI ausente** - Adicionado versão 2.1.0
3. **Controller mal estruturada** - Refatorada completamente
4. **Falta de serviços** - Criado `AnaliseIAService`
5. **Modelos inadequados** - Criados modelos específicos para IA
6. **Program.cs desatualizado** - Registrado novos serviços

### 📦 **NOVOS PACOTES ADICIONADOS:**
```xml
<PackageReference Include="OpenAI" Version="2.1.0" />
```

### 🆕 **NOVOS ARQUIVOS CRIADOS:**

1. **`Models/AnaliseIAModels.cs`** - Modelos para resposta da IA
2. **`Services/AnaliseIAService.cs`** - Serviço especializado em IA
3. **`Controllers/AnaliseAmostragemController.cs`** - Controller refatorada

---

## 📋 ENDPOINTS DISPONÍVEIS

### 🔍 **POST /api/analiseamostragem/analisar**
Análise principal com IA

**Parâmetros:**
- `arquivo` (IFormFile): Arquivo CSV de amostragem
- `instrucoesEspecificas` (string): Instruções adicionais para IA
- `incluirRecomendacoes` (bool): Incluir recomendações (padrão: true)
- `incluirAnaliseRisco` (bool): Incluir análise de risco (padrão: true)
- `maximoRegistros` (int): Máximo de registros (padrão: 1000)

### 📤 **POST /api/analiseamostragem/upload**
Endpoint legado (mantido para compatibilidade)

### 🛠️ **POST /api/analiseamostragem/processar-csv**
Apenas processa CSV sem IA (debug)

### ✅ **GET /api/analiseamostragem/testar-ia**
Testa conexão com OpenAI

---

## 📊 FORMATO DA RESPOSTA

```json
{
  "sucesso": true,
  "mensagemErro": null,
  "praga": {
    "nomePopular": "Lagarta-do-cartucho",
    "nomeCientifico": "Spodoptera frugiperda",
    "descricao": "Praga que ataca principalmente milho...",
    "tipoDano": "Foliar",
    "sintomasCaracteristicos": [
      "Furos nas folhas",
      "Presença de fezes"
    ],
    "confiancaIdentificacao": 85.5
  },
  "recomendacoes": [
    {
      "tipo": "Químico",
      "descricao": "Aplicar inseticida específico",
      "urgencia": "Alta",
      "produtosSugeridos": ["Produto A", "Produto B"],
      "dosagem": "200ml/ha",
      "melhorEpocaAplicacao": "Manhã cedo"
    }
  ],
  "risco": {
    "nivelInfestacao": "Alto",
    "percentualRisco": 75.0,
    "impactoEconomico": "Pode reduzir produtividade em 30%",
    "fatoresRisco": ["Alta umidade", "Temperatura favorável"],
    "tempoReacao": "Imediato - 3 dias"
  },
  "analiseCompleta": "Texto completo da análise...",
  "metricas": {
    "totalRegistrosProcessados": 150,
    "tempoProcessamento": "00:00:04.1234567",
    "tokensUtilizados": 2847,
    "modeloIA": "gpt-4o-mini",
    "processadoEm": "2025-07-02T14:30:00Z"
  }
}
```

---

## 🎯 EXEMPLOS DE TESTE

### **1. Teste de Conexão**
```bash
curl -X GET "https://localhost:7000/api/analiseamostragem/testar-ia"
```

### **2. Análise Básica**
```bash
curl -X POST "https://localhost:7000/api/analiseamostragem/analisar" \
  -H "Content-Type: multipart/form-data" \
  -F "arquivo=@amostragem_pragas.csv" \
  -F "maximoRegistros=500"
```

### **3. Análise com Instruções Específicas**
```bash
curl -X POST "https://localhost:7000/api/analiseamostragem/analisar" \
  -H "Content-Type: multipart/form-data" \
  -F "arquivo=@amostragem_pragas.csv" \
  -F "instrucoesEspecificas=Foque na identificação de pragas em soja e milho. Priorize recomendações de controle biológico."
```

---

## 🔧 CONFIGURAÇÕES

### **appsettings.json:**
```json
{
  "OpenAI": {
    "ApiKey": "sua-chave-configurada",
    "Model": "gpt-4o-mini",
    "MaxTokens": 4000,
    "Temperature": 0.7
  }
}
```

### **Limites Configurados:**
- **Arquivo máximo:** 10MB
- **Registros máximos:** 1000 (configurável)
- **Timeout:** 30 segundos por request

---

## 💰 CUSTOS ESTIMADOS

### **Com gpt-4o-mini:**
- **Por análise:** ~$0.002 USD (menos de R$ 0,02)
- **1000 análises:** ~$2.00 USD (menos de R$ 12,00)
- **Muito econômico** para uso agrícola

---

## 🎨 MELHORIAS IMPLEMENTADAS

### **🧠 IA Especializada:**
- Prompts específicos para pragas agrícolas
- Análise contextual dos dados
- Extração automática de insights
- Recomendações práticas

### **🔧 Arquitetura Melhorada:**
- Separação de responsabilidades (Services)
- Modelos estruturados
- Tratamento de erros robusto
- Logs detalhados

### **📊 Análise Avançada:**
- Identificação de pragas com confiança
- Avaliação de risco automática
- Recomendações por tipo de controle
- Métricas de processamento

### **🛡️ Segurança:**
- Validação de arquivos
- Limites de tamanho
- Tratamento de exceções
- Rate limiting implícito

---

## 🧪 TESTANDO A API

1. **Execute:** `dotnet run`
2. **Acesse:** Swagger UI
3. **Upload:** Arquivo `amostragem_pragas.csv` existente
4. **Veja:** Análise inteligente da IA!

### **Arquivo de Teste Disponível:**
- `amostragem_pragas.csv` 
- `amostragem_pragas - Corrigido.csv`

---

## 🏆 RESULTADO FINAL

✅ **API totalmente funcional** com IA integrada  
✅ **Problemas corrigidos** e arquitetura melhorada  
✅ **Endpoints especializados** para análise agrícola  
✅ **Documentação completa** no Swagger  
✅ **Pronta para produção** com configurações adequadas  

**Sua API AgroTech Safra agora é uma ferramenta inteligente para identificação e manejo de pragas agrícolas!** 🌾🤖