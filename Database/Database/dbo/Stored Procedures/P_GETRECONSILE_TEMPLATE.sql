  
--EXEC P_GETRECONSILE_TEMPLATE   --used reconsile Process    
CREATE PROCEDURE [dbo].[P_GETRECONSILE_TEMPLATE]      
@TEMPLATEID INT=0      
AS      
BEGIN      
    
SELECT    
[RECONSILE_TEMPLATE_ID] AS TemplateId,    
[TEMPLATE_NAME] AS TemplateName,    
[SOURCE_FOLDER_PATH] AS SourceFolder,    
case when ISNULL(@TEMPLATEID, 0) = 0 then (select File_Extension from FileExtension where File_Extension_Id = [SOURCE_FILE_EXTENTION]) else SOURCE_FILE_EXTENTION end AS SourceExtention,    
[SOURCE_COMPLETION_PATH] AS SourceCompletionPath,    
[SOURCE_SUBSTRING_VALUE] AS SourceSubstringValue ,    
case when ISNULL(@TEMPLATEID, 0) = 0 then (select Delimiter_Value from Delimiter where Delimiter_Id = [SOURCE_DELIMITER]) else [SOURCE_DELIMITER] end AS  SourceDelimiter  ,    
[SOURCE_HASHEADER] AS SourceHasHeader ,     
[IS_ACTIVE] AS  IsActive,
Number_Of_Parameter NumberOfParameters ,
TEMPLATE_GROUP_ID  TemplateGroupId  
FROM RECONSILE_TEMPLATE WHERE RECONSILE_TEMPLATE_ID = @TEMPLATEID      
          OR  (ISNULL(@TEMPLATEID, 0) = 0)     
    AND IS_ACTIVE = 1  
	and TEMPLATE_GROUP_ID in (select TEMPLATEGROUP_ID from Template_Group where is_Active = 1)
    order by TEMPLATE_GROUP_ID
END   