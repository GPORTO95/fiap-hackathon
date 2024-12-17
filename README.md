# 🔥 FIAP | Hackathon - Health&Med

O objetivo desse desafio é criar uma aplicação onde seja possível realizar o cadastro de médico, pacientes, horários disponíveis de médicos e realização de consulta de pacientes, testes unitários e pipeline CI/CD.

## :stop_sign: Requisitos
### Funcionais
- Cadastro de médicos
- Autenticação de médicos
- Cadastro/Edição de horários dísponiveis de médicos
- Cadastro de pacientes
- Aunteticação de pacientes
- Busca por médicos
- Agendamento de consultas
- Notificação de consulta marcada
### Não funcionais
- Concorrência de agendamento: O sistema deve garantir que o agendamento seja permitida para um determinado horários.
- Validação de conflito de horários: O sistema deve validar a disponibilidade do horário selecionado e assegurar que não haja sobreposição de horários para consultas agendadas.

## :woman_technologist: Tecnologias
- .NET 9
- Entity Framework Core 9
- FluentValidator
- MediatR
- Teste de unidade
- Unit Of Work
- CQRS

## :building_construction: Arquitetura
> [!NOTE]
> Utilizado um arquitetura separada em Camadas (abaixo explico o que cada uma compõe), assim obedecendo os padrões do DDD como separação de responsabilidade, responsabilidade única, objetos de valores, modelos ricos, ....

- **Application**: CQRS, interfaces para serviços externos, pipeline behaviors e validações de comandos.com fluent validator
- **Domain**: Classes compartilhadas, entidades, objetos de valor, interface de repositorios e mensagens de erros
- **Infrastructure**: Camada de acesso a dados, cache e classes concretas de acesso a serviços externos
- **Application.UnitTests**: Testes unitário para classes de comandos, queries e validações
- **Domain.UnitTests**: Testes unitário para entidades e objetos de valor

## :deciduous_tree: Projeto
<!-- src
    |-- building blocks
    |   |-- Hackathon.HealthMed.Kernel
    |   |   |-- DomainObjects (Aggragate, Value objects compartilhados)
    |   |   |-- Shared (Result pattern)
    |   |-- Hacktahon.HealthMed.Api.Core
    |   |   |-- Identidade (Dados compartilhado de autenticação)
    |-- services
    |   |-- medicos
    |   |   |-- Hackathon.HealtMed.Medico.Api
    |   |   |   |-- Controllers
    |   |   |   |-- Configuration (DI)
    |   |   |-- Hackathon.HealtMed.Medico.Application
    |   |   |   |-- CQRS
    |   |   |   |-- Abstrações (Interfaces de serviços externos)
    |   |   |-- Hackathon.HealtMed.Medico.Domain (Entidades)
    |   |   |   |-- Entidadades
    |   |   |-- Hackathon.HealtMed.Medico.Infrastructure (Banco de dados, serviços externos e cache)
    |   |   |   |-- Data (Acesso a banco)
    |   |   |   |-- Repositorios
    |   |-- pacientes
    |   |   |-- Hackathon.HealthMed.Pacientes.Api (Controller, entidades, validações)
tests
    |--  -->

## :white_check_mark: Tarefas
- [x] Script SQL
- [x] Documentação
- [ ] Endpoints
    - [ ] POST | Autenticação médico
    - [ ] POST | Criação de médico
    - [ ] GET | Médicos paginado
    - [ ] GET | Horarios disponiveis por medico
    - [ ] POST | Criação paciente
    - [ ] POST | Autenticação paciente
- [ ] Testes unitários
- [ ] CI/CD
    

## :bookmark: Métodos
> [!IMPORTANT]
> Propriedades marcadas com o ícone :small_orange_diamond: são de preenchimento obrigatório nos atributos

<details>
    <summary>[Cadastro de médico]</summary>

```http
POST /api/v1/medicos
```

- #### Caso de sucesso
    - Será retornado um status code 200 com o Id cadastrado do médico

- #### Caso de uso
    - Caso o `email` informado já esteja registrado será retornado um BadRequest
    - Caso o `cpf` informado já esteja registrado será retornado um BadRequest
    - Caso o `crm` informado já esteja registrado será retornado um BadRequest

- #### Validação de dados
    - Caso o `nome` informado não seja valido será retornado um BadRequest
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `cpf` informado não seja valido será retornado um BadRequest
    - Caso o `crm` informado não seja valido será retornado um BadRequest

- #### Atributos
    - :small_orange_diamond: **nome** | String: Deve ser informado o nome completo e so será permitido apenas letras, em caso de chars especial ou números será retornado um BadRequest
        - Exemplo válido: Gabriel Teste
        - Exemplo inválido: T3ste
    - :small_orange_diamond: **email** | String: Deve ser informado um e-mail válido
        - Exemplo válido: teste@exemplo.com
        - Exemplo inválido: teste@exemplo
    - :small_orange_diamond: **cpf** | String: Deve ser informado um número de cpf válido sem pontos e traço
        - Exemplo válido: 21644957051
        - Exemplo inválido: 216.449.570-51
    - :small_orange_diamond: **crm** | String: Só será permitido numeros, com número exato de 6 chars
        - Exemplo válido: 1456214
        - Exemplo inválido: 154e45
    - :small_orange_diamond: **senha** | String: Deve ser informado a senha contendo no mínimo 8 chars, 1 letra maiúscula, 1 letra minúscula, números e char especial
        - Exemplo válido: Teste@123
        - Exemplo inválido: Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "nome": "Gabriel Porto",
        "email": "gabriel.porto@teste.com",
        "cpf": "21644957051",
        "crm": "1456214",
        "senha": "Teste123*"
    }
    ```
    - ##### Response - Será retornado um Guid com o Id do médico
    ```
    "28eb0baa-e67a-4f64-86e1-cfa1326301c6"
    ```
     - ##### Caso de uso - crm já cadastrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Médico.CrmJaCadastrado",
        "status": 400,
        "detail": " O crm '123456' iformado já está cadastrado"
    }
    ```
    - ##### Validação - Nome inválido
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Nome.NomeIncompleto",
        "status": 400,
        "detail": "Informe o nome completo"
    }
    ```
</details>
<details>
    <summary>[Listar medicos]</summary>

```http
GET /api/v1/medicos?pagina=1
```

- #### Caso de sucesso
    - Será retornado uma objeto tipo PagedList com dados de paginação

- #### Query Parametros
    - :small_orange_diamond:**pagina** | int: Deve ser informado a página posicionada
    - **pesquisa** | String: Pode ser informado o nome, email ou crm para filtro


- #### Exemplo Response
    - ##### Listagem
    ```json
    {
        "pagina": 1,
        "existeProximaPagina": true,
        "existePaginaAnterior": false,
        "total": 20,
        "lista": [
            {
                "id: ": "62db978f-9999-45c9-9304-2d12554bd038",
                "nome": "Hugo Almeida",
                "email": "hugo.almeida@teste.com",
                "cpf": "21644957051",
                "crm": "1456214",
                "senha": "Teste123*"
            },
            {
                "id: ": "62db978f-9999-45c9-9304-2d12554bd038",
                "nome": "Lucas Rocha",
                "email": "lucas.rocha@teste.com",
                "cpf": "21644957051",
                "crm": "1456214",
                "senha": "Teste123*"
            }
        ]
    }
    ```
</details>
<details>
    <summary>[Listar horarios disponiveis medico]</summary>

```http
GET /api/v1/medicos/{medicoId}/horarios-disponiveis
```

- #### Caso de sucesso
    - Será retornado uma lista com os horarios disponiveis do médico

- #### Route Parametros
    - :small_orange_diamond:**mmedicoId** | int: Deve ser informado o id do médico


- #### Exemplo Response
    - ##### Listagem
    ```json
    [
        {
            "Data": "2025-01-01",
            "horario": "08:00"
        },
        {
            "Data": "2025-01-01",
            "horario": "09:00"
        },
        {
            "Data": "2025-01-01",
            "horario": "09:30"
        }
    ]
    ```
</details>
<details>
    <summary>[Cadastro de pacientes]</summary>

```http
POST /api/v1/pacientes
```

- #### Caso de sucesso
    - Será retornado um status code 200 com o Id cadastrado do paciente

- #### Caso de uso
    - Caso o `email` informado já esteja registrado será retornado um BadRequest
    - Caso o `cpf` informado já esteja registrado será retornado um BadRequest

- #### Validação de dados
    - Caso o `nome` informado não seja valido será retornado um BadRequest
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `cpf` informado não seja valido será retornado um BadRequest

- #### Atributos
    - :small_orange_diamond: **nome** | String: Deve ser informado o nome completo e so será permitido apenas letras, em caso de chars especial ou números será retornado um BadRequest
        - Exemplo válido: Gabriel Teste
        - Exemplo inválido: T3ste
    - :small_orange_diamond: **email** | String: Deve ser informado um e-mail válido
        - Exemplo válido: teste@exemplo.com
        - Exemplo inválido: teste@exemplo
    - :small_orange_diamond: **cpf** | String: Deve ser informado um número de cpf válido sem pontos e traço
        - Exemplo válido: 21644957051
        - Exemplo inválido: 216.449.570-51
    - :small_orange_diamond: **senha** | String: Deve ser informado a senha contendo no mínimo 8 chars, 1 letra maiúscula, 1 letra minúscula, números e char especial
        - Exemplo válido: Teste@123
        - Exemplo inválido: Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "nome": "Gabriel Porto",
        "email": "gabriel.porto@teste.com",
        "cpf": "21644957051",
        "senha": "Teste123*"
    }
    ```
    - ##### Response - Será retornado um Guid com o Id do paciente
    ```
    "28eb0baa-e67a-4f64-86e1-cfa1326301c6"
    ```
     - ##### Caso de uso - email já cadastrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Paciente.EmailJaCadastrado",
        "status": 400,
        "detail": "O email 'teste@exemplo.com' informado já está cadastrado"
    }
    ```
    - ##### Validação - Nome inválido
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Nome.NomeIncompleto",
        "status": 400,
        "detail": "Informe o nome completo"
    }
    ```
</details>