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

<!--## :deciduous_tree: Projeto
 src
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
- [x] Estrutura
    - [x] Classes compartilhadas
    - [x] Value Objects
        - [x] Nome
        - [x] Senha
        - [x] Cpf
        - [x] Email
        - [x] Crm
    - [x] Entidades
    - [x] Conexão com banco de dados
- [ ] Endpoints
    - [x] POST | Autenticação médico
    - [x] POST | Criação de médico
    - [x] POST | Criação de horário disponível de médico
    - [x] PUT | Atualização de horário de médico
    - [ ] GET | Listagem de medicos com horarios disponiveis
    - [ ] POST | Agendamento de paciente e médico
    - [x] POST | Criação paciente
    - [x] POST | Autenticação paciente
- [ ] Testes unitários
    - [x] Value Objects
    - [x] Pacientes
    - [ ] Doutores
- [ ] CI/CD
    

## :bookmark: Métodos
> [!IMPORTANT]
> Propriedades marcadas com o ícone :small_orange_diamond: são de preenchimento obrigatório nos atributos

<details>
    <summary>[Login de medicos]</summary>

```http
POST /api/v1/doctors/login
```

- #### Caso de sucesso
    - Será retornado um status code 200 com token


- #### Validação de dados
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `password` informado não seja valido será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Email | String | Sim | Deve ser informado um e-mail válido | teste@gmail.com | teste@gmail |
| Password | String | Sim | Deve ser informado no mínimo 8 chars, 1 letra maiúscula, 1 letra min´´uscula, 1 numero e 1 char especial | Teste@123 | Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "email": "gabriel.porto@teste.com",
        "password": "Teste123*"
    }
    ```
    - ##### Response - Será retornado um Token
    ```
    "28eb0baa-e67a-4f64-86e1-cfa1326301c6"
    ```
    - ##### Validação - Password inválido
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Password.Empty",
        "status": 400,
        "detail": "Password is empty"
    }
    ```
</details>
<details>
    <summary>[Cadastro de médico]</summary>

```http
POST /api/v1/doctors
```

- #### Caso de sucesso
    - Será retornado um status code 200 com o Id cadastrado do médico

- #### Caso de uso
    - Caso o `email` informado já esteja registrado será retornado um BadRequest
    - Caso o `cpf` informado já esteja registrado será retornado um BadRequest

- #### Validação de dados
    - Caso o `name` informado não seja valido será retornado um BadRequest
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `cpf` informado não seja valido será retornado um BadRequest
    - Caso o `crm` informado não seja valido será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Name | String | Sim | Deve ser informado o nome completo com apenas letras | Gabriel Teste | T3st#
| Email | String | Sim | Deve ser informado um e-mail válido | teste@gmail.com | teste@gmail |
| Cpf | String | Sim | Deve ser informado um cpf válido sem pontos e traço | 21644957051 | 216.449.570-51 |
| Crm | String | Sim | Só será permitido numeros, com número entre 6 e 7 | 1456214 | 154e45 |
| Password | String | Sim | Deve ser informado no mínimo 8 chars, 1 letra maiúscula, 1 letra min´´uscula, 1 numero e 1 char especial | Teste@123 | Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "name": "Gabriel Porto",
        "email": "gabriel.porto@teste.com",
        "cpf": "21644957051",
        "crm": "1456214",
        "password": "Teste123*"
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
    <summary>[Cadastro de horarios de médico]</summary>

```http
POST /api/v1/doctors/schedule
```

- #### Caso de sucesso
    - Será retornado um status code 200 com o Id do horario cadastrado

- #### Caso de uso
    - Caso o `Id` informado não esteja registrado será retornado um NotFound
    - Caso o `date` informado seja uma data menor que a atual, será retornado um BadRequest
    - Caso o `start` ou `end` informado entre em conflito com algum horário cadastrado, será retornado um Conflict
    - Caso o `start` seja maior que o `end` será retornado um BadRequest
    - Caso o `start` ou `end` seja uma data inválida será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| DoctorId | Guid | Sim | Deve ser informado o Id do doutor | 273b548a-63bc-424f-bb6a-0f60052c0f7a | T3st#
| Date | DateOnly | Sim | Deve ser informado uma data válida | "2025-01-01" | "01-12-2029" |
| Start | TimeSpan | Sim | Deve ser informado um horário válido | "09:23" | "15" |
| End | TimeSpan | Sim | Deve ser informado um horário válido | "11:00" | "25" |

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "doctorId": "273b548a-63bc-424f-bb6a-0f60052c0f7a",
        "date": "2025-01-31",
        "start": "09:42",
        "end": "10:00"
    }
    ```
    - ##### Response - Será retornado um Guid com o Id do médico
    ```
    "28eb0baa-e67a-4f64-86e1-cfa1326301c6"
    ```
     - ##### Caso de uso - Doutor não encontrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        "title": "Doctor.NotFound",
        "status": 404,
        "detail": "Doctor not found",
        "traceId": "00-73e350dc2606f3a74e699e599ddcd1fa-cb54e2e1ccc57acd-00"
    }
    ```
    - ##### Caso de uso - Horario já cadastrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        "title": "DoctorSchedule.ScheduleIsNotFree",
        "status": 409,
        "detail": "Doctor schedule is not free.",
        "traceId": "00-3bc071319f22599bb89ade8d0544533a-fe9df2d0d7610588-00"
    }
    ```
</details>
<details>
    <summary>[Atualização de horarios de médico]</summary>

```http
PUT /api/v1/doctors/{doctorScheduleId}/schedule
```

- #### Caso de sucesso
    - Será retornado um status code 204

- #### Caso de uso
    - Caso o `DoctorScheduleId` informado não esteja registrado será retornado um NotFound
    - Caso o `date` informado seja uma data menor que a atual, será retornado um BadRequest
    - Caso o `start` ou `end` informado entre em conflito com algum horário cadastrado, será retornado um Conflict
    - Caso o `start` seja maior que o `end` será retornado um BadRequest
    - Caso o `start` ou `end` seja uma data inválida será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| DoctorScheduleId | Guid | Sim | Deve ser informado o Id do agendamento | 273b548a-63bc-424f-bb6a-0f60052c0f7a | T3st#
| Date | DateOnly | Sim | Deve ser informado uma data válida | "2025-01-01" | "01-12-2029" |
| Start | TimeSpan | Sim | Deve ser informado um horário válido | "09:23" | "15" |
| End | TimeSpan | Sim | Deve ser informado um horário válido | "11:00" | "25" |

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "doctorScheduleId": "273b548a-63bc-424f-bb6a-0f60052c0f7a",
        "date": "2025-01-31",
        "start": "09:42",
        "end": "10:00"
    }
    ```
    - ##### Response - 204 NoContent
    ```
    ```
     - ##### Caso de uso - Doutor não encontrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        "title": "Doctor.NotFound",
        "status": 404,
        "detail": "Doctor not found",
        "traceId": "00-73e350dc2606f3a74e699e599ddcd1fa-cb54e2e1ccc57acd-00"
    }
    ```
    - ##### Caso de uso - Horario já cadastrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        "title": "DoctorSchedule.ScheduleIsNotFree",
        "status": 409,
        "detail": "Doctor schedule is not free.",
        "traceId": "00-3bc071319f22599bb89ade8d0544533a-fe9df2d0d7610588-00"
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
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Pagina | Number | Sim | Deve ser informado a página posicionada | 2 | false |
| Pesquisa | String |Não | Pode ser informado o nome, email ou crm para filtro | João |  |


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
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| MedicoId | Number | Sim | Deve ser informado o Id | 2 | false |


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
POST /api/v1/patients
```

- #### Caso de sucesso
    - Será retornado um status code 200 com o Id cadastrado do paciente

- #### Caso de uso
    - Caso o `email` informado já esteja registrado será retornado um BadRequest
    - Caso o `cpf` informado já esteja registrado será retornado um BadRequest

- #### Validação de dados
    - Caso o `name` informado não seja valido será retornado um BadRequest
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `cpf` informado não seja valido será retornado um BadRequest
    - Caso o `password` informado não seja valido será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Name | String | Sim | Deve ser informado o nome completo com apenas letras | Gabriel Teste | T3st#
| Email | String | Sim | Deve ser informado um e-mail válido | teste@gmail.com | teste@gmail |
| Cpf | String | Sim | Deve ser informado um cpf válido sem pontos e traço | 21644957051 | 216.449.570-51 |
| Passoword | String | Sim | Deve ser informado no mínimo 8 chars, 1 letra maiúscula, 1 letra min´´uscula, 1 numero e 1 char especial | Teste@123 | Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "name": "Gabriel Porto",
        "email": "gabriel.porto@teste.com",
        "cpf": "21644957051",
        "password": "Teste123*"
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
<details>
    <summary>[Login de pacientes]</summary>

```http
POST /api/v1/patients/login
```

- #### Caso de sucesso
    - Será retornado um status code 200 com token


- #### Validação de dados
    - Caso o `email` informado não seja valido será retornado um BadRequest
    - Caso o `password` informado não seja valido será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Email | String | Sim | Deve ser informado um e-mail válido | teste@gmail.com | teste@gmail |
| Password | String | Sim | Deve ser informado no mínimo 8 chars, 1 letra maiúscula, 1 letra min´´uscula, 1 numero e 1 char especial | Teste@123 | Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "email": "gabriel.porto@teste.com",
        "password": "Teste123*"
    }
    ```
    - ##### Response - Será retornado um Token
    ```
    "28eb0baa-e67a-4f64-86e1-cfa1326301c6"
    ```
    - ##### Validação - Password inválido
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "Password.Empty",
        "status": 400,
        "detail": "Password is empty"
    }
    ```
</details>