CREATE PROCEDURE [dbo].[P_UPDATEUSERS]    
@pUser udt_users Readonly  
AS    
BEGIN    
  
 UPDATE U SET [FirstName] = PU.FirstName,[LastName]= PU.LastName,[IsActive]= PU.IsActive,[Company]= PU.Company,[Email]= PU.Email,[MobilePhone]= PU.MobilePhone,[RoleId]= PU.RoleId
 FROM @pUser PU,USERS U WHERE PU.LoginId = U.UserId

  
end   