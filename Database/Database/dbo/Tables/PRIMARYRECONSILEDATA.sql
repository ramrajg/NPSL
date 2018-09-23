﻿CREATE TABLE [dbo].[PRIMARYRECONSILEDATA] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
    [RRN]          NVARCHAR (MAX)  NOT NULL,
    [DATE]         DATE            NOT NULL,
    [AMOUNT]       NUMERIC (18, 2) NULL,
    [RECOCOL4]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL5]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL6]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL7]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL8]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL9]     NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL10]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL11]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL12]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL13]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL14]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL15]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [RECOCOL16]    NVARCHAR (MAX)  DEFAULT ('NA') NOT NULL,
    [FILENAME]     NVARCHAR (200)  NOT NULL,
    [TEMPLATEID]   INT             NOT NULL,
    [ISRECONSILED] BIT             DEFAULT ((0)) NULL,
    [LASTUPDATE]   DATETIME2 (7)   DEFAULT (getdate()) NULL,
    [CREATEDDATE]  DATETIME2 (7)   DEFAULT (getdate()) NULL, 
    [ISPRIMARY] BIT NOT NULL DEFAULT ((0))
);

GO


create TRIGGER [DBO].[UPDATEPRIMARYRECONSILEDATA]
ON [DBO].[PRIMARYRECONSILEDATA]
AFTER UPDATE, INSERT, DELETE
AS


IF EXISTS(SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
BEGIN
	UPDATE DBO.[PRIMARYRECONSILEDATA] SET LASTUPDATE=GETDATE() 
	FROM INSERTED I
	WHERE DBO.[PRIMARYRECONSILEDATA].ID = I.ID
END

IF EXISTS (SELECT * FROM INSERTED) AND NOT EXISTS(SELECT * FROM DELETED)
BEGIN
    UPDATE DBO.[PRIMARYRECONSILEDATA] SET CREATEDDATE=GETDATE() 
	FROM INSERTED I
	WHERE DBO.[PRIMARYRECONSILEDATA].ID = I.ID
END
