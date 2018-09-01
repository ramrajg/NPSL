--P_GETUSERSVALIDATIONMENUMODEL '2','Admin'          
CREATE PROCEDURE [DBO].[P_GETUSERSVALIDATIONMENUMODEL]          
@UserId INT,          
@Password NVARCHAR(MAX)          
AS          
BEGIN          
 IF EXISTS (SELECT USERID FROM USERS WHERE  USERID = @USERID)          
 BEGIN          
  IF EXISTS (SELECT USERID FROM USERS WHERE  USERID = @USERID AND LOGINPASSWORD = @PASSWORD)          
  BEGIN          
        
      
           
SELECT MainMenuName, SM.MAINMENUID as MainMenuId,SubMenuName,RM.SUBMENUID as SubMenuId,CONTROLLER as ControllerName,ACTION as ActionName,RM.ROLEID as RoleId,ROLENAME as RoleName FROM ROLEMENU RM         
JOIN SUBMENU SM ON SM.SUBMENUID = RM.SUBMENUID        
JOIN MAINMENU MM ON SM.MAINMENUID = MM.MAINMENUID        
JOIN ROLEMASTER R ON RM.ROLEID = R.ROLEID        
WHERE RM.ROLEID = 1        
      
  SELECT  UserId,FirstName,LastName,IsActive,Company,Email,MobilePhone,RoleId FROM USERS WHERE LoginId = @USERID AND LOGINPASSWORD = @PASSWORD          
   AND ISACTIVE = 1    
    
   --SELECT   MainMenuName, 
   --SM.MAINMENUID as MainMenuId,
   --SubMenuName,
   --RM.SUBMENUID as SubMenuId
   --,CONTROLLER as ControllerName
   --,ACTION as ActionName
   --,RM.ROLEID as RoleId
   --,ROLENAME as RoleName       
   --,UserId,FirstName,LastName,IsActive,Company,Email,MobilePhone      
         
--   FROM ROLEMENU RM         
--JOIN SUBMENU SM ON SM.SUBMENUID = RM.SUBMENUID        
--JOIN MAINMENU MM ON SM.MAINMENUID = MM.MAINMENUID        
--JOIN ROLEMASTER R ON RM.ROLEID = R.ROLEID        
--join USERS U on U.RoleId = RM.RoleID WHERE LoginId = @USERID AND LOGINPASSWORD = @PASSWORD        
      
  END          
  ELSE          
  BEGIN          
   RAISERROR('Incorrect Password', 16, 1)           
  END          
 END          
 ELSE          
 BEGIN          
    RAISERROR('User Does Not Exist', 16, 1)             
   END          
END