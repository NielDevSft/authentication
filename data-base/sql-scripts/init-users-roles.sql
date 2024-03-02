USE DBLoja;

-- Verificar existência da tabela JwtClaims antes de criar
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JwtClaims' AND TABLE_SCHEMA = '[app]')
BEGIN
    CREATE TABLE [app].[JwtClaims] (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Active BIT NOT NULL,
        CreateAt DATETIME2,
        Removed BIT NOT NULL,
        Subject NVARCHAR(MAX) NOT NULL,
        UpdateAt DATETIME2
    );
END;

-- Verificar existência da tabela Role antes de criar
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Role' AND TABLE_SCHEMA = '[app]')
BEGIN
    CREATE TABLE [app].[Role] (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Active BIT NOT NULL,
        CreateAt DATETIME2,
        JwtClaimsId INT,
        Name NVARCHAR(MAX) NOT NULL,
        Removed BIT NOT NULL,
        UpdateAt DATETIME2,
        FOREIGN KEY (JwtClaimsId) REFERENCES [app].JwtClaims(Id)
    );
END;

-- Verificar existência da tabela User antes de criar
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User' AND TABLE_SCHEMA = '[app]')
BEGIN
    CREATE TABLE [app].[User] (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Active BIT NOT NULL,
        CreateAt DATETIME2,
        Email NVARCHAR(MAX) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Removed BIT NOT NULL,
        UpdateAt DATETIME2,
        Username NVARCHAR(MAX) NOT NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRole' AND TABLE_SCHEMA = '[app]')
BEGIN
    CREATE TABLE [app].[UserRole] (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Active BIT NOT NULL,
        UserId INT,
        RoleId INT,
        Removed BIT NOT NULL,
        UpdateAt DATETIME2,
        CreateAt DATETIME2,
        FOREIGN KEY (UserId) REFERENCES [app].[User](Id),
        FOREIGN KEY (RoleId) REFERENCES [app].[Role](Id)
    );
END;

