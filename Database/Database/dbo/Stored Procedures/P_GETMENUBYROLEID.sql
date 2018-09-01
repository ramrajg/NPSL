﻿--EXEC P_GETMENUBYROLEID '2'
CREATE PROCEDURE [dbo].[P_GETMENUBYROLEID]
@ROLEID INT
AS
BEGIN


SELECT MainMenuName, SM.MAINMENUID as MainMenuId,SubMenuName,RM.SUBMENUID as SubMenuId,CONTROLLER as ControllerName,ACTION as ActionName,RM.ROLEID as RoleId,ROLENAME as RoleName FROM ROLEMENU RM 
JOIN SUBMENU SM ON SM.SUBMENUID = RM.SUBMENUID
JOIN MAINMENU MM ON SM.MAINMENUID = MM.MAINMENUID
JOIN ROLEMASTER R ON RM.ROLEID = R.ROLEID
WHERE RM.ROLEID = @ROLEID


END