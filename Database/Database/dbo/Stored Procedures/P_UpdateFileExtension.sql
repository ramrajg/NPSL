create PROCEDURE [dbo].[P_UpdateFileExtension]          
@pFileExtension udt_fileExtension Readonly        
AS          
BEGIN          
        
UPDATE FE SET [File_Extension] =FileExtensionName
FROM @pFileExtension PFE,FileExtension FE WHERE PFE.FileExtensionId = FE.File_Extension_Id      
end 

