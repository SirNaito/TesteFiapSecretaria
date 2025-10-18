CREATE DATABASE fiap_secretaria;
GO
USE fiap_secretaria;
GO

-- Alunos
IF NOT EXISTS (SELECT * FROM sysobjects a WHERE name='Alunos' AND a.type='U')
BEGIN
    CREATE TABLE Alunos 
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(100) NOT NULL,
        DataNascimento DATE NOT NULL,
        CPF CHAR(11) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Senha NVARCHAR(500) NOT NULL
    );
END
GO

CREATE UNIQUE INDEX UX_Alunos_CPF ON Alunos(CPF);
CREATE UNIQUE INDEX UX_Alunos_Email ON Alunos(Email);

----------------------------------------------------------------------------------

-- Turmas
IF NOT EXISTS (SELECT * FROM sysobjects a WHERE name='Turmas' AND a.type='U')
BEGIN
    CREATE TABLE Turmas 
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(100) NOT NULL,
        Descricao NVARCHAR(250) NOT NULL
    );
END
GO

CREATE UNIQUE INDEX UX_Turmas_Nome ON Turmas(Nome);

----------------------------------------------------------------------------------

-- Matrículas
IF NOT EXISTS (SELECT * FROM sysobjects a WHERE name='Matriculas' AND a.type='U')
BEGIN
    CREATE TABLE Matriculas 
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        AlunoId INT NOT NULL,
        TurmaId INT NOT NULL,
        CONSTRAINT FK_Matriculas_Aluno FOREIGN KEY (AlunoId) REFERENCES Alunos(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Matriculas_Turma FOREIGN KEY (TurmaId) REFERENCES Turmas(Id) ON DELETE CASCADE
    );
END
GO

CREATE UNIQUE INDEX UX_Matriculas_Aluno_Turma ON Matriculas(AlunoId, TurmaId);

----------------------------------------------------------------------------------

-- User Admin 
IF NOT EXISTS (SELECT * FROM sysobjects a WHERE name='Admins' AND a.type='U')
BEGIN
    CREATE TABLE Admins 
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Senha NVARCHAR(500) NOT NULL
    );
END
GO

--Indice Unico dos Admins
CREATE UNIQUE INDEX UX_Admins_Email ON Admins(Email);

-- Criação Registro Admin Inicial (Senha: Admin123 - Hash do .NET 8 Identity)
INSERT INTO Admins (Nome, Email, Senha)
VALUES 
('Admin01', 'admin@fiap.com.br', 'AQAAAAIAAYagAAAAEK8U7rU6T8Q8hE7N6AZhW/ydWJ9U1w4x3Aa4Vxy0zOqKQXzQ5jC4iyt0T1SbN2A3mw==');
GO