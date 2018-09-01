CREATE TABLE [dbo].[RoleMaster] (
    [RoleId]   INT          IDENTITY (1, 1) NOT NULL,
    [RoleName] VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_RoleMaster] PRIMARY KEY CLUSTERED ([RoleId] ASC),
    CONSTRAINT [RoleName_unique] UNIQUE NONCLUSTERED ([RoleName] ASC)
);

