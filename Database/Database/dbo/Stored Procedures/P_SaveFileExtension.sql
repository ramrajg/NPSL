
create PROCEDURE [dbo].[P_SaveFileExtension]          
@pFileExtension udt_fileExtension Readonly        
AS          
BEGIN          
        
        
insert into FileExtension([File_Extension])       
select FileExtensionName from @pFileExtension       
end 

