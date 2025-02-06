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
- Confirmação de consultas

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
- [x] Endpoints
    - [x] POST | Autenticação médico
    - [x] POST | Criação de médico
    - [x] POST | Criação de horário disponível de médico
    - [x] PUT | Atualização de horário de médico
    - [x] GET | Listagem de medicos
    - [x] GET | Listagem de horarios disponiveis por médico
    - [x] POST | Agendamento de paciente e médico
    - [x] PATCH | Aceitação/Recusa de agendamento
    - [x] POST | Criação paciente
    - [x] POST | Autenticação paciente
- [x] Testes unitários
    - [x] Value Objects
    - [x] Pacientes
    - [x] Doutores
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
| Crm | String | Sim | Deve ser informado um crm válido | 123456 | teste |
| Password | String | Sim | Deve ser informado no mínimo 8 chars, 1 letra maiúscula, 1 letra min´´uscula, 1 numero e 1 char especial | Teste@123 | Teste

- #### Exemplo Request
    - ##### Válido
    ```json
    {
        "crm": "123456",
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
    - Caso o `crm` informado já esteja registrado será retornado um BadRequest

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
| Specialty | Enum | Sim | Deve ser informado a especialidade do médico | 1 | Cardiologista
    - 0 = Cardiology
    - 1 = Dermatology
    - 2 = Endocrinology
    - 3 = Gastroenterology
    - 4 = GeneralPractice
    - 5 = Gynecology
    - 6 = InfectiousDiseases
    - 7 = Nephrology
    - 8 = Neurology
    - 9 = Ophthalmology
    - 10 = Orthopedics
    - 11 = Otorhinolaryngology
    - 12 = Pediatrics
    - 13 = Psychiatry
    - 14 = Pulmonology
    - 15 = Radiology
    - 16 = Rheumatology
    - 17 = Urolog

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
    <summary>[Cadastro de horarios por médico]</summary>

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
    - Caso o status do Schedule esteja para Pendente ou Aceito, será retornando um BadRequest

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
GET /api/v1/doctors?page=1
```

- #### Caso de sucesso
    - Será retornado uma objeto tipo PagedList com dados de paginação

- #### Query Parametros
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Page | Number | Sim | Deve ser informado a página posicionada | 2 | false |
| PageSize | Number | Sim | Deve ser informado a quantidade que se deseja obter por página | 10 | false |
| Search | String |Não | Pode ser informado o nome, email, cpf ou crm para filtro | João |  |
| Specialty | Enum | Não | Pode ser informado a especialidade do médico para filtro | 1 | Testet |


- #### Exemplo Response
    - ##### Listagem
    ```json
    {
        "page": 1,
        "pageSize": 10,
        "totalCount": 20,
        "hasNextPage": true,
        "hasPreviousPage": false,
        "items": [
            {
                "doctorId: ": "62db978f-9999-45c9-9304-2d12554bd038",
                "name": "Hugo Almeida",
                "email": "hugo.almeida@teste.com",
                "cpf": "21644957051",
                "crm": "1456214",
                "specialty": 2
            },
            {
                "doctorId: ": "62db978f-9999-45c9-9304-2d12554bd038",
                "name": "Lucas Rocha",
                "email": "lucas.rocha@teste.com",
                "cpf": "21644957051",
                "crm": "1456213",
                "specialty": 1
            }
        ]
    }
    ```
</details>
<details>
    <summary>[Listar horarios disponiveis de medico]</summary>

```http
GET /api/v1/doctors/{doctorId}/available-schedule
```

- #### Caso de sucesso
    - Será retornado uma lista com os horarios disponiveis do médico informado

- #### Caso de uso
    - Caso o `doctorId` informado não esteja cadastrado, será retornado um 404

- #### Query Parametros
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| DoctorId | Guid | Sim | Deve ser informado o id do médico | "62db978f-9999-45c9-9304-2d12554bd038" | false |

- #### Exemplo Response
    - ##### Listagem
    ```json
    [
        {
            "doctorScheduleId: ": "62db978f-9999-45c9-9304-2d12554bd038",
            "date": "2025-01-31",
            "start": "09:42",
            "end": "10:00"
        },
        {
            "doctorScheduleId: ": "62db978f-9999-45c9-9304-2d12554bd038",
            "date": "2025-01-31",
            "start": "10:10",
            "end": "11:00"
        }
    ]
    ```
</details>
<details>
    <summary>[Agendamento de consulta]</summary>

```http
POST /api/v1/doctors/{doctorScheduleId}/{patientId}/appointment
```

- #### Caso de sucesso
    - Será retornado um status code 204 NoContent

- #### Caso de uso
    - Caso o `DoctorScheduleId` informado não esteja registrado será retornado um NotFound
    - Caso o `PatientId` informado não esteja registrado será retornado um NotFound
    - Caso o `DoctorScheduleId` informado não esteja com status de `Free` será retornado um BadRequest

- #### Parametros de rota
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| DoctorScheduleId | Guid | Sim | Deve ser informado o Id do horarios disponivel do doutor | 273b548a-63bc-424f-bb6a-0f60052c0f7a | T3st#
| PatientId | Guid | Sim | Deve ser informado o Id do paciente | 273b548a-63bc-424f-bb6a-0f60052c0f7a | T3st#

- #### Exemplo Request
    - ##### Válido
    ```json (não necessário)
    ```
    - ##### Response
    ```
    ```
     - ##### Caso de uso - Doutor não encontrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        "title": "DoctorSchedule.NotFound",
        "status": 404,
        "detail": "DoctorSchedule not found",
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
    <summary>[Aceitação/Recusa de agendamento]</summary>

```http
PATCH /api/v1/doctors/{doctorScheduleId}/appointment/status:{status}
```

- #### Caso de sucesso
    - Será retornado um status code 204 NoContent

- #### Caso de uso
    - Caso o `DoctorScheduleId` informado não esteja registrado será retornado um NotFound
    - Caso o `DoctorScheduleId` informado não esteja com status de `Pending` será retornado um BadRequest

- #### Parametros de rota
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| DoctorScheduleId | Guid | Sim | Deve ser informado o Id do horarios disponivel do doutor | 273b548a-63bc-424f-bb6a-0f60052c0f7a | T3st# |
| Status | Bool | Sim | Deve ser informado se o agendamento foi aceito ou não | True | 0 |

- #### Exemplo Request
    - ##### Válido
    ```json (não necessário)
    ```
    - ##### Response
    ```
    ```
     - ##### Caso de uso - Doutor não encontrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        "title": "DoctorSchedule.NotFound",
        "status": 404,
        "detail": "DoctorSchedule not found",
        "traceId": "00-73e350dc2606f3a74e699e599ddcd1fa-cb54e2e1ccc57acd-00"
    }
    ```
    - ##### Caso de uso - Horario já cadastrado
    ```json
    {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        "title": "DoctorSchedule.IsNotPending",
        "status": 400,
        "detail": "Doctor schedule is not pending.",
        "traceId": "00-4f57e8edb2a2faf10aea56a48c513d59-9a7f6cc87de71504-00"
    }
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
    - Caso o `cpf` informado não seja valido será retornado um BadRequest
    - Caso o `password` informado não seja valido será retornado um BadRequest

- #### Atributos
| Propriedade | Tipo | Obrigatório | Descrição | Exemplo válido | Exemplo inválido |
|----|----|----|----|----|----|
| Email | String | Não | Deve ser informado um e-mail válido | teste@gmail.com | teste@gmail |
| Cpf | String | Não | Deve ser informado um cpf válido | 83351678002 | 11122233344 |
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