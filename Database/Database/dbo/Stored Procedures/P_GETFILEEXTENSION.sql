--EXEC P_GETFILEEXTENSION '2'    
CREATE PROCEDURE P_GETFILEEXTENSION 
@EXTENSTIONID INT=0      
AS      
BEGIN      
    
    
SELECT  File_Extension_Id FileExtensionId,   
        File_Extension  FileextensionName   
       FROM FileExtension     
        WHERE File_Extension_Id = @EXTENSTIONID      
          OR  (ISNULL(@EXTENSTIONID, 0) = 0)    
    
END 