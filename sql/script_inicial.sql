USE PublicEnterpriseDb

CREATE TABLE [Doctors] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [PasswordHash] varchar(100) NOT NULL,
    [Crm] varchar(7) NOT NULL,
    [Specialty] int NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([Id])
);

CREATE TABLE [DoctorSchedules] (
    [Id] uniqueidentifier NOT NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NULL,
    [Date] DATE NOT NULL,
    [Start] TIME NOT NULL,
    [End] TIME NOT NULL,
    [Status] INT NOT NULL,
    [Reason] VARCHAR(100) NULL,
    [Price] DECIMAL (6,2) NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Schedule] PRIMARY KEY ([Id])
);

CREATE TABLE [Patients] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [PasswordHash] varchar(100) NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
);