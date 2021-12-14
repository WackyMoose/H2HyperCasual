-- Version 1.0

--CREATE DATABASE Tanx
--GO

USE Tanx
GO

CREATE TABLE [Users] 
(
	[PlayerId] int NOT NULL PRIMARY KEY,
	[PlayerName] NVARCHAR(32) NOT NULL,
	[Password] NVARCHAR(32) NOT NULL
);

CREATE TABLE [Players]
(
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[PlayerName] NVARCHAR(32) NOT NULL,
	[Kills] INT NOT NULL DEFAULT 0,
	[Deaths] INT NOT NULL DEFAULT 0
);

CREATE TABLE [Matches]
(
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[WinnerPlayerId] INT NOT NULL,
	[PlayTime] INT NOT NULL
);

CREATE TABLE [PlayerMatches] 
(
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[PlayerId] int NOT NULL,
	[MatchId] int NOT NULL,
);

CREATE TABLE [Logs] (

   [Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
   [Message] nvarchar(max) NULL,
   [MessageTemplate] nvarchar(max) NULL,
   [Level] nvarchar(128) NULL,
   [TimeStamp] datetime NOT NULL,
   [Exception] nvarchar(max) NULL,
   [Properties] nvarchar(max) NULL
);