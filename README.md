# 🚀 Projeto Challenger API (.NET)

Este repositório contém a implementação de uma **API desenvolvida em .NET 8**, estruturada em camadas de acordo com princípios de **Domain-Driven Design (DDD)**.  
O projeto foi criado como parte da Sprint 4 com objetivo de aplicar boas práticas de desenvolvimento, versionamento e arquitetura de software.

---

## 🎯 Objetivos do Projeto
- Implementar uma API REST em **.NET** com arquitetura organizada em camadas.
- Aplicar conceitos de **Domain-Driven Design (DDD) e SOLID**.
- Estruturar as camadas **Domain, Application, Infrastructure e API**.
- Permitir fácil execução local para testes e evolução da aplicação.
---

## 🚀 Tecnologias Utilizadas

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **MySQL / SQL Server **
- **Swagger + Versionamento**
- **xUnit + Moq** (para testes)
- **Clean Architecture + DDD**
  
---

## 🛠️ Estrutura do Projeto

```
NET-MOTTU-main/
│
├── Challenger.API/            # Camada de apresentação (Controllers e Startup)
├── Challenger.Application/    # Casos de uso e regras de aplicação
├── Challenger.Domain/         # Entidades e regras de negócio
├── Challenger.Infrastructure/ # Persistência e integrações externas
│
├── Challenger.sln             # Arquivo da solução .NET
├── global.json
└── .gitignore
```

---

📘 Versionamento do Swagger

O projeto possui versionamento de API configurado no Swagger.
Cada versão da API é documentada separadamente e pode ser acessada através de:

/swagger/v1/swagger.json
/swagger/v2/swagger.json


Isso garante que novas versões da API possam ser publicadas sem quebrar compatibilidade com clientes antigos.

---

## ▶️ Como Rodar Localmente

### 📌 Pré-requisitos
- [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- Banco de dados MySql disponível

### 📥 Clonar o repositório
```bash
git clone <url-do-repo>
cd NET-MOTTU-main
```

### ⚙️ Restaurar dependências
```bash
dotnet restore
```
### Configure a string de coexão no arquivo appsettings.json:
  ```json
    "ConnectionStrings": {
    "MotoGridDB":"server=localhost;port=3306;database=MotoGridDB;user=root;password=SuaSenhaSegura;"
    }
  ```

### ▶️ Executar a API
```bash
cd Challenger.API
dotnet run
```

A API ficará disponível por padrão em:
```
https://localhost:5001
http://localhost:5000
```

### ✅ Testar a aplicação
Você pode testar os endpoints usando:
- [Postman](https://www.postman.com/)
- `curl` no terminal
- Navegador para os endpoints GET

### 🔐 Autenticação JWT

A aplicação utiliza JSON Web Token (JWT) para autenticação.

## 🔸 Geração do Token

Para gerar um token JWT, envie uma requisição POST para:

```bash
POST /api/auth/login
```

## 📤 Exemplo de requisição:
``` json
{
  "email": "victorhugo@gmail.com",
  "senha": "Fiapm1234"
}
```
## 📥 Exemplo de resposta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```
## 🔒 Como usar o Token

Após gerar o token, inclua-o no cabeçalho das próximas requisições:

Authorization: Bearer <seu_token_jwt>

---

## 🧾 Endpoints protegidos

Os endpoints de Motos, Pátios e demais recursos exigem autenticação via token.
Sem o token válido, a API retornará 401 Unauthorized.

---
### 📦 Exemplos de Requisição

## POST /api/patios
  ```json
  {
    "nome": "Pátio Centro",
    "cidade": "São Paulo",
    "capacidade": 50
  }
  ```
## GET /api/patios/cidade/São Paulo
    Retorna todos os pátios localizados na cidade de São Paulo.

---

## POST /api/motos
```json
  {
  "placa": "XYZ-1234",
  "modelo": "1, 2 ou 3 ",
  "patioId": "GUID_DO_PATIO"
}
```
## GET /api/motos/placa/XYZ1234
      Retorna os dados da moto com a placa especificada.
---

**POST /api/users**

```json
{
  "username": "DustSams",
  "email" : "victorhugo@gmail.com",
  "senha" : "Fiapm1234"
}
```
---

## 🧪 Testes Automatizados

O projeto contém testes unitários utilizando xUnit e Moq.

### ▶️ Executar todos os testes
```bash
cd Challenger.Tests
dotnet test
```
---
## 🧠 Tecnologias de Teste

- xUnit → Framework de testes padrão do .NET

- Moq → Criação de mocks e simulação de dependências

- Arrange / Act / Assert → Estrutura padrão de escrita dos testes

Exemplo de teste incluído:

CreateMotoUseCaseTests.cs — valida a criação de motos e uso dos enums ModeloMoto e StatusMoto.


## 👥 Integrantes

- **Gabriel Gomes Mancera** - RM: 555427  
- **Juliana de Andrade Sousa** - RM: 558834  
- **Victor Hugo Carvalho Pereira** - RM: 558550  

---

## 📌 Observações
- Este projeto é voltado para execução **local**.
- O JWT depende apenas de usuários previamente cadastrados (autenticação por email e senha)
- Testes unitários cobrem casos de uso essenciais, permitindo evolução segura do código.
  
