CREATE TABLE [dbo].[RoleMenu] (
    [SubMenuId] INT NOT NULL,
    [RoleID]    INT NOT NULL,
    CONSTRAINT [fk_Role_Menu_Id] FOREIGN KEY ([SubMenuId]) REFERENCES [dbo].[SubMenu] ([SubMenuId]),
    CONSTRAINT [fk_Role_Menu_Role_Id] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[RoleMaster] ([RoleId])
);

