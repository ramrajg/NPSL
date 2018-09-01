create PROCEDURE [dbo].[P_UpdateTemplateGroup]        
@pTemplateGroup udt_templateGroup Readonly       
AS        
BEGIN        
      
UPDATE TG SET [Template_Group_Name] =TemplateGroupName,[Is_Active]=IsTemplateGroupActive
FROM @pTemplateGroup PTG,Template_Group TG WHERE PTG.TemplateGroupId = TG.TemplateGroup_Id    
end 