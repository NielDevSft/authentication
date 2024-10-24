USE [master]
GO

IF DB_ID('DBAuthentication') IS NOT NULL
  set noexec on 

CREATE DATABASE [DBAuthentication];
GO

USE [DBAuthentication]
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

CREATE LOGIN [AdmAuthentication] WITH PASSWORD = 'AuthCation@247'
GO

CREATE USER [AdmAuthentication] FOR LOGIN [AdmAuthentication] WITH DEFAULT_SCHEMA=[app]
GO

EXEC sp_addrolemember N'db_owner', N'AdmAuthentication'
GO