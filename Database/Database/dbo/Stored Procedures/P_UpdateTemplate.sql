create PROCEDURE [dbo].[P_UpdateTemplate]      
@pTemplate udt_template Readonly    
AS      
BEGIN      
    
UPDATE RT SET [Template_Name] =TemplateName,[Source_Folder_Path]=SourceFolder,[Source_File_Extention]=SourceExtention,[Source_Completion_Path]=SourceCompletionPath,[Source_Substring_Value]=SourceSubstringValue,[Template_Group_id] = TemplateGroupId,
[Source_Delimiter]=SourceDelimiter,[Source_HasHeader]=SourceHasHeader,[Is_Active]=IsActive,[Number_Of_Parameter] = NumberOfParameters,[Template_Group_Status] = TemplateGroupStatus
FROM @pTemplate PRT,Reconsile_Template RT WHERE PRT.TemplateId = RT.Reconsile_Template_Id  
end 