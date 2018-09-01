--EXEC P_GETUSER '2'
CREATE PROCEDURE [P_GetUser]  
@USERID INT=0  
AS  
BEGIN  


SELECT  UserId,FirstName,LastName,IsActive,Company,Email,MobilePhone,USR.RoleId,RM.RoleName FROM USERS USR
inner join RoleMaster RM on RM.RoleId = USR.RoleId WHERE USERID = @USERID  
          OR  (ISNULL(@USERID, 0) = 0)  -- AND ISACTIVE = 1

END  