--EXEC P_GETROLEBYID
CREATE PROCEDURE P_GetRoleById    
@ROLEID INT=0    
AS    
BEGIN    
  
  
SELECT  RoleId,RoleName FROM RoleMaster USR  
WHERE RoleId = @ROLEID    
          OR  (ISNULL(@ROLEID, 0) = 0)  -- AND ISACTIVE = 1  
  
END 