# DBZ API

API ASP.NET Core para cadastro e consulta de personagens inspirados em Dragon Ball Z.

O projeto principal está nesta pasta (`src`) e usa uma organização em camadas para separar entrada HTTP, regras de aplicação, domínio e persistência.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- EF Core InMemory
- Swagger / OpenAPI
- xUnit para testes

## Estrutura

```text
src/
├── DBZ.csproj
├── DBZ.presentation/
│   ├── Program.cs
│   ├── Controllers/
│   ├── DTOs/
│   ├── appsettings.json
│   └── appsettings.Development.json
├── DBZ.application/
│   ├── Abstractions/
│   ├── DTOs/
│   ├── Races/
│   └── Services/
├── DBZ.domain/
│   └── entities/
└── DBZ.infrastructure/
    ├── Persistence/
    └── Repositories/
```

## Camadas

`DBZ.presentation`

Camada de entrada da aplicação. Contém o `Program.cs`, configuração da API, Swagger e controllers HTTP.

`DBZ.application`

Camada de aplicação. Contém serviços, contratos e estratégias para descrição de personagens por raça.

`DBZ.domain`

Camada de domínio. Contém as entidades principais, como `Personagem` e `Saiyan`.

`DBZ.infrastructure`

Camada de infraestrutura. Contém o `AppDbContext`, configuração do Entity Framework Core e implementação do repositório.

## Modelo principal

A entidade `Personagem` possui:

- `Id`: identificador do personagem.
- `Name`: nome do personagem.
- `Power`: poder principal numérico.
- `Race`: raça do personagem, usada para escolher textos, bônus de poder e transformações disponíveis.
- `Transformation`: transformação atual do personagem.
- `Description`: descrição livre.

A entidade `Saiyan` herda de `Personagem`, define `Race` como `Saiyan` e pode usar `PowerLevel` para enriquecer a descrição.

## Estratégia por raça

A aplicação usa `IRaceStrategy` para aplicar comportamento por raça sem criar uma classe para cada raça.

Os dados ficam centralizados em perfis (`RaceProfile`), que definem:

- texto de descrição;
- bônus de poder;
- transformações disponíveis.

Exemplos:

- `Saiyan` permite a transformação `Super Saiyan`.
- `Namekusei` permite a transformação `Gigante`.
- `Humano` não permite essas transformações.

O serviço `PersonagemService` usa a strategy registrada na injeção de dependência e retorna `Greeting`, `EffectivePower` e `AvailableTransformations` em `PersonagemResult`.

## Banco de dados

A aplicação usa o provider `Microsoft.EntityFrameworkCore.InMemory` com o banco `DbzDb`.

Os dados ficam apenas em memória durante a execução da aplicação.

## Como executar

A partir da raiz do repositório:

```bash
dotnet restore ProjectDBZ/ProjectDBZ.sln
dotnet build ProjectDBZ/ProjectDBZ.sln
dotnet run --project src/DBZ.csproj
```

Ou, a partir da pasta `src`:

```bash
dotnet restore DBZ.csproj
dotnet build DBZ.csproj
dotnet run --project DBZ.csproj
```

Em ambiente de desenvolvimento, o Swagger fica disponível no endereço exibido pelo `dotnet run`, normalmente:

```text
https://localhost:<porta>/swagger
```

## Endpoints

Base route:

```text
/api/personagens
```

### Criar personagem

```http
POST /api/personagens
Content-Type: application/json
```

Exemplo:

```json
{
  "name": "Goku",
  "power": 80,
  "race": "Saiyan",
  "description": "Protagonista da saga"
}
```

Resposta esperada:

- `201 Created`

### Buscar personagem por ID

```http
GET /api/personagens/{id}
```

Respostas esperadas:

- `200 OK`
- `404 Not Found`

### Listar personagens

```http
GET /api/personagens
```

Resposta esperada:

- `200 OK`

### Atualizar personagem

```http
PUT /api/personagens/{id}
Content-Type: application/json
```

Exemplo:

```json
{
  "name": "Goku",
  "power": 120,
  "race": "Saiyan",
  "description": "Muito forte"
}
```

Respostas esperadas:

- `200 OK`
- `400 Bad Request` quando a raça é desconhecida.
- `404 Not Found`

### Remover personagem

```http
DELETE /api/personagens/{id}
```

Resposta esperada:

- `204 No Content`

### Transformar personagem

```http
POST /api/personagens/{id}/transformations/{transformation}
```

Ou:

```http
POST /api/personagens/{id}/transformations
Content-Type: application/json
```

```json
{
  "transformation": "Super Saiyan"
}
```

Exemplo com rota:

```http
POST /api/personagens/00000000-0000-0000-0000-000000000000/transformations/Super%20Saiyan
```

Respostas esperadas:

- `200 OK`
- `400 Bad Request` quando a raça não permite a transformação.
- `404 Not Found`

### Listar raças

```http
GET /api/races
```

Resposta esperada:

- `200 OK`

Exemplo de item retornado:

```json
{
  "race": "Namekusei",
  "powerBonus": 8,
  "transformations": ["Gigante"]
}
```

## Testes

Os testes ficam fora de `src`, na pasta:

```text
../ProjectDBZ.Tests
```

Para executar a partir da raiz do repositório:

```bash
dotnet test ProjectDBZ/ProjectDBZ.sln
```

Os testes usam `Microsoft.EntityFrameworkCore.InMemory` para validar operações sem depender de SQL Server.

## Observação de estado atual

O projeto principal está em `src/`, mas a solução fica em `ProjectDBZ/ProjectDBZ.sln` e referencia `../src/DBZ.csproj`.
