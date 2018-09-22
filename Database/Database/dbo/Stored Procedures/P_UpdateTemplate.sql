create PROCEDURE [dbo].[P_UpdateTemplate]      
@pTemplate udt_template Readonly    
AS      
BEGIN      
declare @TemplateGroupID  int,@IsPrimary int,@hasPrimary int,@templateId int
select @TemplateGroupID =  TEMPLATEGROUPID,@IsPrimary = IsPrimary,@templateId = TemplateId from    @PTEMPLATE 
select @hasPrimary = COUNT(1) FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @TemplateGroupID AND ISPRIMARY = 1 and Reconsile_Template_Id <> @templateId

IF ((@hasPrimary = 0 and @IsPrimary =1) or (@hasPrimary > 0 and @IsPrimary =0) or (@hasPrimary = 0 and @IsPrimary = 0))  
BEGIN     
UPDATE RT SET [Template_Name] =TemplateName,[Source_Folder_Path]=SourceFolder,[Source_File_Extention]=SourceExtention,[Source_Completion_Path]=SourceCompletionPath,[Source_Substring_Value]=SourceSubstringValue,[Template_Group_id] = TemplateGroupId,
[Source_Delimiter]=SourceDelimiter,[Source_HasHeader]=SourceHasHeader,[Is_Active]=IsActive,[Number_Of_Parameter] = NumberOfParameters,[Template_Group_Status] = TemplateGroupStatus,RT.[IsPrimary] = PRT.IsPrimary
FROM @pTemplate PRT,Reconsile_Template RT WHERE PRT.TemplateId = RT.Reconsile_Template_Id  
END
ELSE
BEGIN
RAISERROR('Template Group mentioned cannot have 2 Primary Template',16,1);
END        
END 