CREATE PROCEDURE [p_DeleteTemplate]    
@pTemplateId INT    
AS    
BEGIN    
    
DELETE FROM Reconsile_Template WHERE Reconsile_Template_Id = @pTemplateId    
    
END 