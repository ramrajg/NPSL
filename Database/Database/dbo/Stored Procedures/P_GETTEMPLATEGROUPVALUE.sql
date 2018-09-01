--EXEC P_GETTEMPLATEGROUPVALUE 0,1       
CREATE PROCEDURE [dbo].[P_GETTEMPLATEGROUPVALUE]     
@TEMPLATEGROUPID INT=0,
@OnlyActive bit = 1         
AS          
BEGIN          

Declare @SSQL varchar(8000)    
Declare @isActive varchar(8000)
set  @isActive = case when  @OnlyActive = 1 then ' Is_Active = 1' end    
print @TEMPLATEGROUPID  
set @SSQL  = 'SELECT  TemplateGroup_Id TemplateGroupId,       
        Template_Group_Name  TemplateGroupName,
		Is_Active        IsTemplateGroupActive
       FROM [Template_Group]         
        WHERE TemplateGroup_Id =' + cast(@TEMPLATEGROUPID as varchar(10)) + ' OR  (ISNULL('+ cast(@TEMPLATEGROUPID as varchar(10)) +', 0) = 0)'

		if(@OnlyActive =1)
		begin
		set @SSQL  = @SSQL + ' and Is_Active = 1'
		end
	   exec (@SSQL)

END 


