create database dbClinica;
GO
use dbClinica;
GO

--Tabela tutor
CREATE TABLE Tutor (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cpf VARCHAR(64) NOT NULL UNIQUE,
    Telefone VARCHAR(20) NOT NULL,
    DataNascimento DATE NOT NULL
);
GO

-- Tabela Pet
CREATE TABLE Pet (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
	Especie varchar(50) not null,
	Raca varchar(50) not null,
	TutorId int not null,
	Constraint FK_Pet_Tutor FOREIGN KEY (TutorId)
        REFERENCES Tutor(Codigo)
);
GO

-- Tabela Medico
CREATE TABLE Medico (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Crmv VARCHAR(20) NOT NULL UNIQUE,
    Especialidade VARCHAR(50) NOT NULL
);
GO

-- Tabela Agendamento
CREATE TABLE Agendamento (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    DataAgendamento DATETIME NOT NULL,
    TutorId INT NOT NULL,
    PetId INT NOT NULL,
	MedicoId INT NOT NULL,
    CONSTRAINT FK_Agendamento_Tutor FOREIGN KEY (TutorId)
        REFERENCES Tutor(Codigo),
    CONSTRAINT FK_Agendamento_Pet FOREIGN KEY (PetId)
        REFERENCES Pet(Codigo),
	CONSTRAINT FK_Agendamento_Medico FOREIGN KEY (MedicoId)
        REFERENCES Medico(Codigo)
);
GO

--Inserindo Dados Iniciais para Teste
INSERT INTO Tutor (Nome, Cpf, Telefone, DataNascimento) VALUES
('Maria Oliveira', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '111.222.333-44'), 2), '(11) 98888-7777', '1990-05-15'),
('João Souza',     CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '555.666.777-88'), 2), '(11) 97777-6666', '1985-10-20'),
('Ana Lima',       CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '999.888.777-66'), 2), '(11) 96666-5555', '1998-03-08');

INSERT INTO Pet (Nome, Especie, Raca, TutorId) VALUES
('Rex',  'Cachorro', 'Labrador', 1),
('Mimi', 'Gato',     'Siamês',   2),
('Thor', 'Cachorro', 'Poodle',   3);

INSERT INTO Agendamento (DataAgendamento, TutorId, PetId, MedicoId) VALUES
('2026-12-02T10:00:00', 3, 3, 1),
('2026-11-30T14:30:00', 2, 2, 2),
('2026-12-01T09:00:00', 1, 1, 3);
GO

INSERT INTO Medico (Nome, Crmv, Especialidade) VALUES
('Dra. Helena Rios','CRMV/SP 123456','Clínica Geral'),
('Dr. Roberto Alves','CRMV/SP 654321','Cirurgia'),
('Dra. Paula Mendes','CRMV/SP 789012','Dermatologia');
GO