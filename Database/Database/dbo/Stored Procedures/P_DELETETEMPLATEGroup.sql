CREATE PROCEDURE P_DELETETEMPLATEGroup      
@pTemplateGroupId INT      
AS      
BEGIN      
      
DELETE FROM Template_Group WHERE TemplateGroup_Id = @pTemplateGroupId      
      
END 