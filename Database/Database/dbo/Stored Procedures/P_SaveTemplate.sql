﻿create PROCEDURE [dbo].[P_SaveTemplate]      
@pTemplate udt_template Readonly    
AS      
BEGIN      
declare @TemplateGroupID  int,@IsPrimary int,@hasPrimary int
select @TemplateGroupID =  TEMPLATEGROUPID,@IsPrimary = IsPrimary from    @PTEMPLATE 
select @hasPrimary = COUNT(1) FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @TemplateGroupID AND ISPRIMARY = 1
 
IF ((@hasPrimary = 0 and @IsPrimary =1) or (@hasPrimary > 0 and @IsPrimary =0) or (@hasPrimary = 0 and @IsPrimary =0))  
BEGIN 
	INSERT INTO RECONSILE_TEMPLATE ([TEMPLATE_NAME],[SOURCE_FOLDER_PATH],[SOURCE_FILE_EXTENTION],[SOURCE_COMPLETION_PATH],[SOURCE_SUBSTRING_VALUE],[SOURCE_DELIMITER],[SOURCE_HASHEADER]
	,[IS_ACTIVE],[NUMBER_OF_PARAMETER],[TEMPLATE_GROUP_ID],[TEMPLATE_GROUP_STATUS],[IsPrimary],[AmoutWithDecimal])     
	SELECT TEMPLATENAME,SOURCEFOLDER,SOURCEEXTENTION,SOURCECOMPLETIONPATH,SOURCESUBSTRINGVALUE,SOURCEDELIMITER,SOURCEHASHEADER
	,ISACTIVE,NUMBEROFPARAMETERS,TEMPLATEGROUPID,TEMPLATEGROUPSTATUS,IsPrimary,AmoutWithDecimal FROM @PTEMPLATE      
END
ELSE
BEGIN
RAISERROR('Template Group mentioned cannot have 2 Primary Template',16,1);
END        
END 