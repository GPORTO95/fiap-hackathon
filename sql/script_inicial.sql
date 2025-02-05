USE PublicEnterpriseDb

CREATE TABLE [Doctors] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Cpf] varchar(11) NOT NULL,
    [PasswordHash] varchar(100) NOT NULL,
    [Crm] varchar(7) NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([Id])
);

CREATE TABLE [DoctorSchedules] (
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
    CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
);

CREATE TABLE [Appointment] (
    [Id] uniqueidentifier NOT NULL,
    [DoctorScheduleId] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Appointments] PRIMARY KEY ([Id])
);
