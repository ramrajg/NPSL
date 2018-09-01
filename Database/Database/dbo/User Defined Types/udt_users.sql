CREATE TYPE [dbo].[udt_users] AS TABLE (
    [FirstName]     NVARCHAR (MAX) NOT NULL,
    [LastName]      NVARCHAR (MAX) NOT NULL,
    [IsActive]      BIT            NULL,
    [Company]       NVARCHAR (MAX) NULL,
    [LoginId]       NVARCHAR (MAX) NOT NULL,
    [LoginPassword] NVARCHAR (MAX) NULL,
    [Email]         NVARCHAR (MAX) NOT NULL,
    [MobilePhone]   NVARCHAR (MAX) NOT NULL,
    [RoleId]        INT            NOT NULL);

