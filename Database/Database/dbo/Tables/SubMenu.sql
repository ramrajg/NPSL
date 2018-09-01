CREATE TABLE [dbo].[SubMenu] (
    [SubMenuId]   INT            IDENTITY (1, 1) NOT NULL,
    [SubMenuName] NVARCHAR (MAX) NOT NULL,
    [Controller]  NVARCHAR (MAX) NOT NULL,
    [Action]      NVARCHAR (MAX) NOT NULL,
    [MainMenuId]  INT            NOT NULL,
    [LastUpdate]  DATETIME2 (7)  DEFAULT (getdate()) NULL,
    [CreatedDate] DATETIME2 (7)  DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_SubMenu] PRIMARY KEY CLUSTERED ([SubMenuId] ASC),
    CONSTRAINT [fk_Menu_Id] FOREIGN KEY ([MainMenuId]) REFERENCES [dbo].[MainMenu] ([MainMenuId])
);


GO



CREATE TRIGGER dbo.updateSubMenu
ON dbo.[SubMenu]
AFTER UPDATE 
AS 
UPDATE dbo.[SubMenu] SET LastUpdate=GETDATE() 
FROM Inserted i
WHERE dbo.[SubMenu].SubMenuId = i.SubMenuId
