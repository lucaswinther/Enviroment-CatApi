/*
    Script para criação das tabelas em um banco de dados SQL Server
*/

-- Caso o database não exista, cria

IF NOT Exists(
    SELECT Name
    FROM sys.databases
    WHERE name = 'thecatdb' 
)
BEGIN
    CREATE DATABASE thecatdb
END
GO

USE thecatdb
GO

-- Caso as tabelas já existam, exclui
--------------------------------------------------------------------

IF OBJECT_ID('ImageUrlCategory', 'u') IS NOT NULL BEGIN
  DROP TABLE ImageUrlCategory
END
GO

IF OBJECT_ID('ImageUrlBreeds', 'u') IS NOT NULL BEGIN
  DROP TABLE ImageUrlBreeds
END
GO

IF OBJECT_ID('ImageUrl', 'u') IS NOT NULL BEGIN
  DROP TABLE ImageUrl
END
GO

IF OBJECT_ID('Category', 'u') IS NOT NULL BEGIN
  DROP TABLE Category
END
GO

IF OBJECT_ID('Breeds', 'u') IS NOT NULL BEGIN
  DROP TABLE Breeds
END
GO

IF OBJECT_ID('LogEvent', 'u') IS NOT NULL BEGIN
  DROP TABLE LogEvent
END
GO

-- Cria as tabelas necessárias
--------------------------------------------------------------------

CREATE TABLE Breeds
(
    BreedsId VARCHAR(80) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Origin VARCHAR(255),
    Temperament VARCHAR(255),
    Description VARCHAR(1024)
)
GO

ALTER TABLE Breeds ADD
    CONSTRAINT PK_Breeds
    PRIMARY KEY CLUSTERED (BreedsId)
GO

CREATE TABLE Category
(
    CategoryId INT NOT NULL,
    Name VARCHAR(255) NOT NULL
)
GO

ALTER TABLE Category ADD
    CONSTRAINT PK_Category
    PRIMARY KEY CLUSTERED (CategoryId)
GO

CREATE TABLE ImageUrl
(
    ImageUrlId VARCHAR(80) NOT NULL,
    Url VARCHAR(512) NOT NULL,
    Width int,
    Height int
)
GO

ALTER TABLE ImageUrl ADD
    CONSTRAINT PK_ImageUrl
    PRIMARY KEY CLUSTERED (ImageUrlId)
GO

CREATE TABLE ImageUrlBreeds
(
    ImageUrlId VARCHAR(80) NOT NULL,
    BreedsId VARCHAR(80) NOT NULL
)
GO

ALTER TABLE ImageUrlBreeds ADD
    CONSTRAINT PK_ImageUrlBreeds
    PRIMARY KEY CLUSTERED (ImageUrlId, BreedsId)
GO

ALTER TABLE ImageUrlBreeds ADD
    CONSTRAINT FK_ImageUrlBreeds_ImageUrl
    FOREIGN KEY (ImageUrlId)
    REFERENCES ImageUrl (ImageUrlId)
GO

ALTER TABLE ImageUrlBreeds ADD
    CONSTRAINT FK_ImageUrlBreeds_Breeds
    FOREIGN KEY (BreedsId)
    REFERENCES Breeds (BreedsId)
GO

CREATE TABLE ImageUrlCategory
(
    ImageUrlId VARCHAR(80) NOT NULL,
    CategoryId INT NOT NULL
)
GO

ALTER TABLE ImageUrlCategory ADD
    CONSTRAINT PK_ImageUrlCategory
    PRIMARY KEY CLUSTERED (ImageUrlId, CategoryId)
GO

ALTER TABLE ImageUrlCategory ADD
    CONSTRAINT FK_ImageUrlCategory_ImageUrl
    FOREIGN KEY (ImageUrlId)
    REFERENCES ImageUrl (ImageUrlId)
GO

ALTER TABLE ImageUrlCategory ADD
    CONSTRAINT FK_ImageUrlCategory_Category
    FOREIGN KEY (CategoryId)
    REFERENCES Category (CategoryId)
GO

CREATE TABLE LogEvent
(
    LogEventId INT NOT NULL IDENTITY(1,1),
    EventDate DATETIME NOT NULL,
    EventTypeId INT NOT NULL,
    EventType VARCHAR(60) NOT NULL,
    MethodName VARCHAR(255) NOT NULL,
    ExecutionTime BIGINT NOT NULL,
    ExecutionTimeFrmt VARCHAR(12) NOT NULL,
    Description VARCHAR(1024)
)
GO

ALTER TABLE LogEvent ADD
    CONSTRAINT PK_LogEvent
    PRIMARY KEY CLUSTERED (LogEventId)
GO

CREATE INDEX IDX_AK_LogEvent_EventType ON LogEvent
    (EventDate, EventType)
GO
