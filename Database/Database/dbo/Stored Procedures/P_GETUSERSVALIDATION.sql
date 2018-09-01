--P_GETUSERSVALIDATION '3','Admin'      
CREATE PROCEDURE [DBO].[P_GETUSERSVALIDATION]      
@UserId INT,      
@Password NVARCHAR(MAX)      
AS      
BEGIN      
 IF EXISTS (SELECT USERID FROM USERS WHERE  USERID = @USERID)      
 BEGIN      
  IF EXISTS (SELECT USERID FROM USERS WHERE  USERID = @USERID AND LOGINPASSWORD = @PASSWORD)      
  BEGIN      
   SELECT  UserId,FirstName,LastName,IsActive,Company,Email,MobilePhone,USR.RoleId,RoleName FROM USERS USR 
   inner join RoleMaster RM on RM.RoleId = USR.RoleId WHERE UserId = @USERID AND LOGINPASSWORD = @PASSWORD      
   AND ISACTIVE = 1     
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