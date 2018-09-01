create PROCEDURE [dbo].[P_SaveTemplateGroup]        
@pTemplateGroup udt_templateGroup Readonly      
AS        
BEGIN        
      
      
insert into Template_Group ([Template_Group_Name],[Is_Active])     
select TemplateGroupName,IsTemplateGroupActive from @pTemplateGroup      
end 