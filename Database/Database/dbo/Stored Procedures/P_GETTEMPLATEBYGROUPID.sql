CREATE PROCEDURE [dbo].[P_GETTEMPLATEBYGROUPID]     
@TemplateGroupId INT=0      
AS      
BEGIN      
    
    
SELECT  Reconsile_Template_Id as TemplateId,   
        Template_Name as   TemplateName ,  
        Source_Folder_Path SourceFolder,   
        case when ISNULL(@TemplateGroupId, 0) = 0 then (select File_Extension from FileExtension where File_Extension_Id = [SOURCE_FILE_EXTENTION]) else SOURCE_FILE_EXTENTION end AS SourceExtention,   
     Source_Completion_Path  SourceCompletionPath,  
        Source_Substring_Value  SourceSubstringValue,  
  Source_Delimiter SourceDelimiter ,  
        Source_HasHeader SourceHasHeader ,  
        Is_Active IsActive,  
  Template_Group_Id TemplateGroupId,  
  IsPrimary,  
  ConditionQuery,  
  case when ISNULL(@TemplateGroupId, 0) = 0 then (select Template_Group_Name  from Template_Group where TemplateGroup_Id = [Template_Group_id])  else '' end AS TemplateGroupName,   
  Template_Group_Status TemplateGroupStatus,  
  Number_Of_Parameter NumberOfParameters  
  FROM Reconsile_Template USR    
        WHERE Template_Group_id = @TemplateGroupId      
         -- OR  (ISNULL(@TemplateGroupId, 0) = 0)   
		 AND Is_Active = 1 and IsPrimary <> 1
    order by Template_Name  
    
    
END

