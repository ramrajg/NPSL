CREATE PROCEDURE p_DeleteTemplateGroup      
@pTemplateGroupId INT      
AS      
BEGIN
IF NOT EXISTS(SELECT 1 FROM Reconsile_Template WHERE Template_Group_id = @pTemplateGroupId)      
BEGIN  
	DELETE FROM Template_Group WHERE TemplateGroup_Id = @pTemplateGroupId        
END
ELSE
BEGIN
	RAISERROR('Cannot Delete Group Since Group Template Already Associated With Template',16,1);
END        
END 