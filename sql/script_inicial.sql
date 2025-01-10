USE PublicEnterpriseDb

CREATE TABLE [Medicos] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [Crm] varchar(7) NOT NULL,
    [SenhaHash] varchar(100) NOT NULL,
    CONSTRAINT [PK_Medicos] PRIMARY KEY ([Id])
);

CREATE TABLE [HorariosMedicos] (
    [Id] uniqueidentifier NOT NULL,
    [MedicoId] uniqueidentifier NOT NULL,
    [Data] DATE NOT NULL,
    [Horario] TIME NOT NULL,
    [Disponivel] BIT NOT NULL,
    CONSTRAINT [PK_Horarios] PRIMARY KEY ([Id])
);

CREATE TABLE [Patients] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [PasswordHash] varchar(100) NOT NULL,
    CONSTRAINT [PK_Pacientes] PRIMARY KEY ([Id])
);

CREATE TABLE [Consultas] (
    [Id] uniqueidentifier NOT NULL,
    [HorarioMedicoId] uniqueidentifier NOT NULL,
    [PacienteId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Consultas] PRIMARY KEY ([Id])
);
