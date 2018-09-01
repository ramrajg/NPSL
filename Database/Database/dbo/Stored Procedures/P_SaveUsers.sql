CREATE PROCEDURE [dbo].[P_SaveUsers]  
@pUser udt_users Readonly
AS  
BEGIN  


insert into Users ([FirstName],[LastName],[IsActive],[Company],[LoginId],[LoginPassword],[Email],[MobilePhone],[RoleId])
select [FirstName],[LastName],[IsActive],[Company],[LoginId],[LoginPassword],[Email],[MobilePhone],[RoleId] from @pUser

end 
