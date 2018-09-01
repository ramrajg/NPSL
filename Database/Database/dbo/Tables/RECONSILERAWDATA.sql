﻿CREATE TABLE [dbo].[RECONSILERAWDATA] (
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
    [CREATEDDATE]  DATETIME2 (7)   DEFAULT (getdate()) NULL
);

