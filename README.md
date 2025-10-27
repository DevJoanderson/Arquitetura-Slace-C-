✅ README.md — Arquitetura Slice com C# + Minimal APIs + MediatR
# 🧩 Arquitetura Slice com C# (.NET) — Sistema de Pedidos

Este projeto implementa uma API para gerenciamento de **Pedidos (Orders)** utilizando a **Arquitetura Slice**, um modelo de organização onde o código é separado por *funcionalidade* ao invés de camadas técnicas.

Isso torna o código:
- Mais fácil de **entender**
- Mais simples de **manter**
- Natural para **evoluir**

---

## 🧱 Tecnologias Utilizadas

| Tecnologia | Função |
|-----------|--------|
| **.NET 8 / Minimal APIs** | Estrutura da API |
| **MediatR (CQRS)** | Separa intenção (Command/Query) da execução (Handler) |
| **FluentValidation** | Validação clara e desacoplada |
| **Entity Framework Core** | Persistência de dados |
| **Banco InMemory** | Para desenvolvimento e testes rápidos |
| **Swagger / OpenAPI** | Documentação interativa |

---

## 🧠 O que são Slices?

Ao invés de separar arquivos por tipo (Controllers, Services, Repositories...),  
a Arquitetura Slice separa o código **por contexto funcional**.



Features
└── Orders
├── Create
├── GetById
├── List
└── Delete


Cada Slice contém tudo o que aquela funcionalidade precisa:

| Arquivo | Papel |
|--------|-------|
| **Command / Query** | Representa a intenção (o que queremos fazer) |
| **Validator** | Garante que os dados recebidos são válidos |
| **Handler** | A lógica que realmente executa a ação |
| **Endpoint** | Expõe a funcionalidade para o mundo via HTTP |

---

## 📦 Entidade Principal

```csharp
public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

🔥 Funcionalidades Implementadas (CRUD Completo)
Verbo	Rota	Descrição
POST	/orders	Criar um novo pedido
GET	/orders/{id}	Buscar um pedido pelo ID
GET	/orders	Listar pedidos com filtro, paginação e ordenação
DELETE	/orders/{id}	Remover um pedido existente
🧩 Fluxo de Execução (Create como exemplo)

O usuário envia um JSON para POST /orders

A API cria um CreateOrderCommand

O Validator verifica se os dados são válidos

O Handler salva no banco via AppDbContext

O Endpoint retorna 201 Created com o Id

Representação simplificada:

HTTP Request → Command → Validator → Handler → DbContext → Response

🗄️ Banco de Dados

Estamos usando:

UseInMemoryDatabase("AppDb")


Isso permite:

Desenvolvimento rápido

Zero configuração

Ótimo para testes

Pode ser trocado facilmente depois por SQLite / SQL Server / PostgreSQL.

🚀 Como executar o projeto
cd DevStore-Api/SliceSample/Api
dotnet run


Abra no navegador:

https://localhost:XXXX/swagger

🧭 Estrutura do Projeto (resumo)
DevStore-Api/
 └── SliceSample/
      └── Api/
          ├── AppDb/
          │   └── AppDbContext.cs
          ├── Domain/
          │   └── Order.cs
          ├── Features/
          │   └── Orders/
          │       ├── Create/
          │       ├── GetById/
          │       ├── List/
          │       └── Delete/
          └── Program.cs

🎯 Próximos Passos (se quiser evoluir)
Funcionalidade	Benefício
PUT /orders/{id}	Completar CRUD com atualização
Pipeline de Validação MediatR	Remove validação manual dos endpoints (Clean Architecture)
Mudar InMemory → SQLite	Persistência real
🏁 Conclusão

Você agora possui uma API completa, organizada e moderna usando:

Arquitetura Slice

CQRS com MediatR

Validação com FluentValidation

Minimal APIs

EF Core
