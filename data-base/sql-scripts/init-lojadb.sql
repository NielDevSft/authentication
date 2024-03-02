USE [master]
GO

IF DB_ID('DBLoja') IS NOT NULL
  set noexec on 

CREATE DATABASE [DBLoja];
GO

USE [DBLoja]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE LOGIN [migrator] WITH PASSWORD = 'migrator123!'
GO

CREATE SCHEMA app
GO

CREATE USER [migrator] FOR LOGIN [migrator] WITH DEFAULT_SCHEMA=[app]
GO

EXEC sp_addrolemember N'db_owner', N'migrator'
GO

CREATE LOGIN [AdmLoja] WITH PASSWORD = 'LojaSenha@247'
GO

CREATE USER [AdmLoja] FOR LOGIN [AdmLoja] WITH DEFAULT_SCHEMA=[app]
GO

EXEC sp_addrolemember N'db_owner', N'AdmLoja'
GO