
--EXEC P_GETTEMPLATE '0'  
CREATE PROCEDURE [dbo].[P_GETTEMPLATE]   
@TEMPLATEID INT=0    
AS    
BEGIN    
  
  
SELECT  Reconsile_Template_Id as TemplateId, 
        Template_Name as   TemplateName ,
        Source_Folder_Path SourceFolder, 
        case when ISNULL(@TEMPLATEID, 0) = 0 then (select File_Extension from FileExtension where File_Extension_Id = [SOURCE_FILE_EXTENTION]) else SOURCE_FILE_EXTENTION end AS SourceExtention, 
	    Source_Completion_Path  SourceCompletionPath,
        Source_Substring_Value  SourceSubstringValue,
		Source_Delimiter SourceDelimiter ,
        Source_HasHeader SourceHasHeader ,
        Is_Active IsActive,
		Template_Group_Id TemplateGroupId,
		IsPrimary,
		case when ISNULL(@TEMPLATEID, 0) = 0 then (select Template_Group_Name  from Template_Group where TemplateGroup_Id = [Template_Group_id])  else '' end AS TemplateGroupName, 
		Template_Group_Status TemplateGroupStatus,
		Number_Of_Parameter NumberOfParameters  FROM Reconsile_Template USR  
        WHERE Reconsile_Template_Id = @TEMPLATEID    
          OR  (ISNULL(@TEMPLATEID, 0) = 0)  
		  order by TemplateGroupName
		  --AND Is_Active = 1  
  
END 
