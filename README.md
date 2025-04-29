# MonolitoBackend - API de Gerenciamento de Categorias e Produtos
## Visão Geral
API RESTful desenvolvida em ASP.NET Core para gerenciamento de categorias e produtos, seguindo arquitetura em camadas e integração com PostgreSQL em containers Docker.

## Tecnologias Utilizadas

- ASP.NET Core 6+
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Docker

### Pré-requisitos

- Docker versão >= 20.10.7
- Docker Compose versão >= 1.29.2

### Executando a API

1. No arquivo docker-compose.yml, configure as variáveis de ambiente (caso necessário, como senhas de banco de dados ou strings de conexão).

2. Execute os containers:
docker-compose up -d

3. Aplique as migrações no banco de dados:
docker-compose exec api dotnet ef database update

## Endpoints da API

### Categorias

| Método | Endpoint                       | Descrição                                   | Parâmetros               |
|--------|--------------------------------|--------------------------------------------|--------------------------|
| GET    | `/categories`                  | Lista todas as categorias                  | `page`, `pageSize` (opc) |
| GET    | `/categories/{id}`             | Obtém uma categoria pelo ID                | `id` (obrigatório)       |
| POST   | `/categories`                  | Cria uma nova categoria                    | -                        |
| PUT    | `/categories/{id}`             | Atualiza uma categoria existente           | `id` (obrigatório)       |
| DELETE | `/categories/{id}`             | Remove uma categoria                       | `id` (obrigatório)       |
| GET    | `/categories/{id}/products`    | Lista produtos de uma categoria            | `id` (obrigatório)       |

### Produtos

| Método | Endpoint                               | Descrição                           | Parâmetros               |
|--------|----------------------------------------|------------------------------------|--------------------------|
| GET    | `/products`                            | Lista todos os produtos            | `page`, `pageSize` (opc) |
| GET    | `/products/{id}`                       | Obtém um produto pelo ID           | `id` (obrigatório)       |
| GET    | `/products/by-category/{categoryId}`   | Lista produtos por categoria       | `categoryId` (obrigatório) |
| POST   | `/products`                            | Cria um novo produto               | -                        |
| PUT    | `/products/{id}`                       | Atualiza um produto                | `id` (obrigatório)       |
| DELETE | `/products/{id}`                       | Remove um produto                  | `id` (obrigatório)       |

### 🔢 Códigos de Status HTTP

| Código | Status                | Descrição                     | Ocorrência típica          |
|--------|-----------------------|-------------------------------|---------------------------|
| 200    | OK                    | Requisição bem-sucedida       | GET, PUT bem-sucedidos     |
| 201    | Created               | Recurso criado com sucesso    | POST bem-sucedido          |
| 204    | No Content            | Recurso removido com sucesso  | DELETE bem-sucedido        |
| 400    | Bad Request           | Dados inválidos               | Validação falhou           |
| 404    | Not Found             | Recurso não encontrado        | ID inválido               |
| 500    | Internal Server Error | Erro no servidor              | Exceção não tratada       |

### Modelo de Requisição

Categoria (POST/PUT)
{
  "name": "string",
  "description": "string"
}

Produto (POST/PUT)
{
  "name": "string",
  "price": "number",
  "categoryId": "number"
}

### Modelo de Resposta

Categoria
{
  "id": "number",
  "name": "string",
  "description": "string",
  "productsCount": "number"
}

Produto
{
  "id": "number",
  "name": "string",
  "price": "number",
  "categoryId": "number",
  "category": "string"
}


#### Exemplo de requisição GET para obter todas as categorias
curl -X GET http://localhost:5106/api/categories

#### Exemplo de requisição GET para obter uma categoria específica pelo ID
curl -X GET http://localhost:5106/api/categories/1

#### Exemplo de requisição POST para criar uma nova categoria
curl -X POST http://localhost:5106/api/categories     -H "Content-Type: application/json"     -d '{"name": "Nova Categoria", "description": "Descrição da nova categoria"}'

#### Exemplo de requisição PUT para atualizar uma categoria existente
curl -X PUT http://localhost:5106/api/categories/1     -H "Content-Type: application/json"     -d '{"name": "Categoria Atualizada", "description": "Descrição atualizada da categoria"}'

#### Exemplo de requisição DELETE para remover uma categoria pelo ID
curl -X DELETE http://localhost:5106/api/categories/1

#### Exemplo de requisição GET para obter todos os produtos
curl -X GET http://localhost:5106/api/products

#### Exemplo de requisição GET para obter um produto específico pelo ID
curl -X GET http://localhost:5106/api/products/1

#### Exemplo de requisição POST para criar um novo produto
curl -X POST http://localhost:5106/api/products     -H "Content-Type: application/json"     -d '{"name": "Novo Produto", "price": 100.00, "categoryId": 1}'

#### Exemplo de requisição PUT para atualizar um produto existente
curl -X PUT http://localhost:5106/api/products/1     -H "Content-Type: application/json"     -d '{"name": "Produto Atualizado", "price": 120.00, "categoryId": 1}'

#### Exemplo de requisição DELETE para remover um produto pelo ID
curl -X DELETE http://localhost:5106/api/products/1

# Estrutura de Pastas do Projeto MonolitoBackend
## Visão Geral do Projeto
MonolitoBackend/
├── MonolitoBackend.Api/          # Camada de API
├── MonolitoBackend.Core/         # Camada de Domínio/Núcleo
├── MonolitoBackend.Infrastructure/ # Camada de Infraestrutura
├── docker-compose                # Configuração Docker
└── MonolitoBackend.sln           # Solução Visual Studio

### Detalhamento por Camada
1. MonolitoBackend.Api (Camada de Apresentação)
MonolitoBackend.Api/
├── Controllers/          # Controladores da API
├── DTOs/                 # Objetos de Transferência de Dados
├── Properties/           # Configurações de assembly
├── MappingProfile/       # Configurações de mapeamento (AutoMapper)
├── appsettings.json      # Configurações da aplicação
├── appsettings.Development.json # Configs para ambiente de desenvolvimento
├── MonolitoBackend.Api.http # Arquivos de teste HTTP (provavelmente)
├── Program.cs            # Ponto de entrada da aplicação
├── bin/                  # Binários compilados
└── obj/                  # Objetos temporários de compilação
2. MonolitoBackend.Core (Camada de Domínio/Núcleo)
MonolitoBackend.Core/
├── Entities/             # Entidades de domínio
├── Repositories/        # Interfaces de repositórios
├── Services/            # Lógica de negócios/services
├── bin/                 # Binários compilados
└── obj/                 # Objetos temporários de compilação
3. MonolitoBackend.Infrastructure (Camada de Infraestrutura)
MonolitoBackend.Infrastructure/
├── Data/                # Contexto de banco de dados
├── Migrations/         # Migrações do Entity Framework
├── Repositories/       # Implementações concretas de repositórios
├── bin/                # Binários compilados
└── obj/                # Objetos temporários de compilação
