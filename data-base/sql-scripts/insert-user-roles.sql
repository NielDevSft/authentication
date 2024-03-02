USE DBLoja; -- Substitua DBLoja pelo nome real do seu banco de dados

-- Inserir usuário Admin
DECLARE @adminJwtClaimsId INT;

INSERT INTO [app].[JwtClaims] ([Active], [CreateAt], [Removed], [Subject], [UpdateAt])
VALUES (1, GETDATE(), 0, 'AdminSubject', GETDATE());

SET @adminJwtClaimsId = SCOPE_IDENTITY();

INSERT INTO [app].[Role] ([Active], [CreateAt], [JwtClaimsId], [Name], [Removed], [UpdateAt])
VALUES (1, GETDATE(), @adminJwtClaimsId, 'ROLE_ADMIN', 0, GETDATE());

INSERT INTO [app].[User] ([Active], [CreateAt], [Email], [PasswordHash], [Removed], [UpdateAt], [Username])
VALUES (1, GETDATE(), 'admin@example.com', '21232f297a57a5a743894a0e4a801fc3', 0, GETDATE(), 'admin');

-- Inserir usuário Employee
DECLARE @employeeJwtClaimsId INT;

INSERT INTO [app].[JwtClaims] ([Active], [CreateAt], [Removed], [Subject], [UpdateAt])
VALUES (1, GETDATE(), 0, 'EmployeeSubject', GETDATE());

SET @employeeJwtClaimsId = SCOPE_IDENTITY();

INSERT INTO [app].[Role] ([Active], [CreateAt], [JwtClaimsId], [Name], [Removed], [UpdateAt])
VALUES (1, GETDATE(), @employeeJwtClaimsId, 'ROLE_USER', 0, GETDATE());

INSERT INTO [app].[User] ([Active], [CreateAt], [Email], [PasswordHash], [Removed], [UpdateAt], [Username])
VALUES (1, GETDATE(), 'employee@example.com', 'ee11cbb19052e40b07aac0ca060c23ee', 0, GETDATE(), 'employee');

INSERT INTO [app].[UserRole] ([Active], [CreateAt], UserId, RoleId, [Removed], [UpdateAt])
VALUES
    (1, GETDATE(), 1, 1, 0, GETDATE()), -- Admin possui a ROLE_ADMIN
    (1, GETDATE(), 2, 2, 0, GETDATE()); -- Employee possui a ROLE_USER
 