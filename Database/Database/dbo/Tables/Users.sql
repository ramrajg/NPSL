﻿CREATE TABLE [dbo].[Users] (
    [UserId]        INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR (MAX) NOT NULL,
    [LastName]      NVARCHAR (MAX) NOT NULL,
    [IsActive]      BIT            DEFAULT ((0)) NULL,
    [Company]       NVARCHAR (MAX) NULL,
    [LoginId]       NVARCHAR (MAX) NULL,
    [LoginPassword] NVARCHAR (MAX) NOT NULL,
    [Email]         NVARCHAR (MAX) NOT NULL,
    [MobilePhone]   NVARCHAR (MAX) NOT NULL,
    [RoleId]        INT            NOT NULL,
    [LastUpdate]    DATETIME2 (7)  DEFAULT (getdate()) NULL,
    [CreatedDate]   DATETIME2 (7)  DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [fk_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[RoleMaster] ([RoleId])
);


GO


create TRIGGER [DBO].[UPDATEUSERS]
ON [DBO].[USERS]
AFTER UPDATE, INSERT, DELETE
AS


IF EXISTS(SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
BEGIN
	UPDATE DBO.[USERS] SET LASTUPDATE=GETDATE() ,LOGINID = I.USERID
	FROM INSERTED I
	WHERE DBO.[USERS].USERID = I.USERID
END

IF EXISTS (SELECT * FROM INSERTED) AND NOT EXISTS(SELECT * FROM DELETED)
BEGIN
    UPDATE DBO.[USERS] SET LOGINID = I.USERID
	FROM INSERTED I
	WHERE DBO.[USERS].USERID = I.USERID
END


            