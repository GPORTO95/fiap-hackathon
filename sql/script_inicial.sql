USE PublicEnterpriseDb

CREATE TABLE [Doctors] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [PasswordHash] varchar(100) NOT NULL,
    [Crm] varchar(7) NOT NULL,
    CONSTRAINT [PK_Medicos] PRIMARY KEY ([Id])
);

CREATE TABLE [DoctorSchedule] (
    [Id] uniqueidentifier NOT NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [Date] DATE NOT NULL,
    [Start] TIME NOT NULL,
    [End] TIME NOT NULL,
    [Available] BIT NOT NULL,
    CONSTRAINT [PK_Schedule] PRIMARY KEY ([Id])
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
