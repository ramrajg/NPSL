CREATE TABLE [dbo].[MainMenu] (
    [MainMenuId]   INT            IDENTITY (1, 1) NOT NULL,
    [MainMenuName] NVARCHAR (450) NOT NULL,
    [LastUpdate]   DATETIME2 (7)  DEFAULT (getdate()) NULL,
    [CreatedDate]  DATETIME2 (7)  DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_MainMenu] PRIMARY KEY CLUSTERED ([MainMenuId] ASC),
    UNIQUE NONCLUSTERED ([MainMenuName] ASC)
);


GO

CREATE TRIGGER dbo.updateMainMenu
ON dbo.[MainMenu]
AFTER UPDATE 
AS 
UPDATE dbo.[MainMenu] SET LastUpdate=GETDATE() 
FROM Inserted i
WHERE dbo.[MainMenu].MainMenuId = i.MainMenuId
