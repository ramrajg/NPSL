create PROCEDURE [dbo].[P_SaveTemplate]      
@pTemplate udt_template Readonly    
AS      
BEGIN      
    
    
insert into Reconsile_Template ([Template_Name],[Source_Folder_Path],[Source_File_Extention],[Source_Completion_Path],[Source_Substring_Value],[Source_Delimiter],[Source_HasHeader],[Is_Active],[Number_Of_Parameter],[Template_Group_id],[Template_Group_Status])   
select TemplateName,SourceFolder,SourceExtention,SourceCompletionPath,SourceSubstringValue,SourceDelimiter,SourceHasHeader,IsActive,NumberOfParameters,TemplateGroupId,TemplateGroupStatus from @pTemplate    
    
end 