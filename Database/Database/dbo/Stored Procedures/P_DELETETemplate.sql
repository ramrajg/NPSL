CREATE PROCEDURE [P_DELETETemplate]    
@pTemplateId INT    
AS    
BEGIN    
    
DELETE FROM Reconsile_Template WHERE Reconsile_Template_Id = @pTemplateId    
    
END 