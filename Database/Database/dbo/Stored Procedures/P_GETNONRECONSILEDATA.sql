﻿CREATE PROCEDURE [dbo].[P_GetNonReconsileData]      
@GROUPTEMPLATEID INT, 
@SELECTEDTEMPLATEID INT,     
@FROMDATE DATETIME,      
@TODATE DATETIME       
AS            
BEGIN            
IF OBJECT_ID('tempdb..##TEMPRECONSILEREPORT') IS NOT NULL DROP TABLE ##TEMPRECONSILEREPORT    
           
DECLARE @PRIMARYTEMPLATEID INT,          
@PRIMARYTEMPLATENAME VARCHAR(30),          
@@COUNT INT = 0,          
@@SQLQUERY VARCHAR(MAX),      
@@SQLQUERYFORRECORDCOUNT nvarchar(max),    
@@SQLQUERYFORRECORDDELETE nvarchar(max),         
@TEMPLATEID INT,          
@TEMPALTENAME VARCHAR(100),          
@CONDITIONQUERY VARCHAR(MAX),      
@PrimaryRecordCount int =0,     
@NonPrimaryRecordCount int   = 0,  
@RecordCountDIFF INT =0   
DECLARE @intFlag INT  
          
SELECT @PRIMARYTEMPLATEID = RECONSILE_TEMPLATE_ID,@PRIMARYTEMPLATENAME = TEMPLATE_NAME  FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID AND ISPRIMARY =1          
          
SET @@SQLQUERY = 'CREATE TABLE ##TEMPRECONSILEREPORT(Id int,[RRNNUMBER] NVARCHAR(MAX),AMOUNT NUMERIC(18,2),[DATE] DateTime,[TemplateId] int,[TemplateName] VARCHAR(400)
,[IsReconsile] BIT,[ReconsileType] varchar(10),[ReconsileDesc] varchar(200),TemplateSpilt int,IsPrimary BIT)'          
EXEC(@@SQLQUERY)      
    
        
          
SET @@SQLQUERY = ''       
SET @@SQLQUERYFORRECORDCOUNT = ''   
SET @@SQLQUERYFORRECORDDELETE = ''       
DECLARE RECONSILEREPORT_CURSOR CURSOR            
LOCAL  FORWARD_ONLY  FOR            
SELECT DISTINCT TEMPLATEID,RT.TEMPLATE_NAME,ISNULL(CONDITIONQUERY,'') AS CONDITIONQUERY          
FROM RECONSILERAWDATA            
JOIN RECONSILE_TEMPLATE RT ON RECONSILE_TEMPLATE_ID = TEMPLATEID          
WHERE TEMPLATEID IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID AND Reconsile_Template_Id = @SELECTEDTEMPLATEID )          
OPEN RECONSILEREPORT_CURSOR            
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME,@CONDITIONQUERY          
WHILE @@FETCH_STATUS = 0            
BEGIN            
IF @@COUNT = 0           
BEGIN   
 SET @intFlag = 1    
 SET @PrimaryRecordCount = 0       
 SET @NonPrimaryRecordCount = 0  
 SET @RecordCountDIFF = 0  
 SET @@SQLQUERY = 'INSERT INTO ##TEMPRECONSILEREPORT '        
 SET @@SQLQUERY = @@SQLQUERY +' SELECT Id,RRN RRNNUMBER,AMOUNT,[DATE], TemplateId,'''+ @PRIMARYTEMPLATENAME +''' TemplateName,IsReconsiled  IsReconsile,ReconsileType,ReconsileDesc,'''+CAST(@@COUNT + 1 AS VARCHAR(11)) +'''  TemplateSpilt  ,CAST(''True'' AS
 BIT)  IsPrimary   
 FROM PRIMARYRECONSILEDATA WHERE TemplateId in ( '+CAST(@TEMPLATEID AS VARCHAR(10)) +', '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10)) +') 
 And Amount <> 0 
 and ID not in (select PrimaryId from  RECONSILERAWDATA WHERE ISRECONSILED =1  AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +' And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''')
 
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''    
     
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY          
 END      
    
 SET @@SQLQUERYFORRECORDCOUNT = 'SELECT  @PrimaryRecord = COUNT(*) OVER () FROM PRIMARYRECONSILEDATA WHERE TemplateId in ( '+CAST(@TEMPLATEID AS VARCHAR(10)) +', '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10)) +')  and  
 Amount <> 0 and
 ID not in (select PrimaryId from  RECONSILERAWDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +'   
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''')  
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''      
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERYFORRECORDCOUNT = @@SQLQUERYFORRECORDCOUNT +' AND ' + @CONDITIONQUERY          
 END     
     
 EXEC sp_executesql @@SQLQUERYFORRECORDCOUNT, N'@PrimaryRecord int out', @PrimaryRecordCount out   
   
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '          
 SET @@SQLQUERY = @@SQLQUERY +' SELECT Id,RRN,AMOUNT,[DATE],'+CAST(@TEMPLATEID AS VARCHAR(10)) +'  TemplateId,'''+ @TEMPALTENAME +''' [TemplateName],IsReconsiled IsReconsile
 ,ReconsileType,ReconsileDesc,'''+CAST(@@COUNT + 2 AS VARCHAR(11)) +''' TemplateSpilt ,CAST(''False'' AS BIT) IsPrimary FROM RECONSILERAWDATA WHERE ISRECONSILED = 0  
 and Amount <> 0   
 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +' And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''        
      
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY          
 END   
   
 SET @@SQLQUERYFORRECORDCOUNT = 'SELECT  @NonPrimaryRecord = COUNT(*) OVER () FROM RECONSILERAWDATA WHERE ISRECONSILED = 0   and Amount <> 0  AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +' 
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''        
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERYFORRECORDCOUNT = @@SQLQUERYFORRECORDCOUNT +' AND ' + @CONDITIONQUERY          
 END     
     
 EXEC sp_executesql @@SQLQUERYFORRECORDCOUNT, N'@NonPrimaryRecord int out', @NonPrimaryRecordCount out   
  
 IF(@NONPRIMARYRECORDCOUNT =0)  
 BEGIN  
  SET @@SQLQUERYFORRECORDDELETE = @@SQLQUERYFORRECORDDELETE + ' DELETE FROM ##TEMPRECONSILEREPORT WHERE TemplateSpilt = '''+CAST(@@COUNT + 1 AS VARCHAR(11)) +''''  
 END  
  
 IF(@PRIMARYRECORDCOUNT > @NONPRIMARYRECORDCOUNT)  
 BEGIN  
  if(@NONPRIMARYRECORDCOUNT !=0)  
  BEGIN  
   SET @RecordCountDIFF = @PRIMARYRECORDCOUNT - @NONPRIMARYRECORDCOUNT  
  -- WHILE (@intFlag <=@RecordCountDIFF)  
  --  BEGIN  
  --   SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '          
  --   SET @@SQLQUERY = @@SQLQUERY +' SELECT 0,'''',0,GETDATE(), '''','''','''','''','''','''+CAST(@@COUNT + 2 AS VARCHAR(11)) +''' TemplateSpilt,CAST(''True'' AS BIT) IsPrimary '    
  --   SET @intFlag = @intFlag + 1  
  --  END  
  END  
 END  
END          
ELSE          
BEGIN    
 SET @intFlag = 1    
 SET @PrimaryRecordCount = 0       
 SET @NonPrimaryRecordCount = 0  
 SET @RecordCountDIFF = 0        
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '          
 SET @@SQLQUERY = @@SQLQUERY +' SELECT Id,RRN,AMOUNT,[DATE], TemplateId,'''+ @PRIMARYTEMPLATENAME +''' TemplateName,IsReconsiled  IsReconsile,ReconsileType
 ,ReconsileDesc,'''+CAST(@@COUNT + 2 AS VARCHAR(11)) +''' TemplateSpilt,CAST(''True'' AS BIT) IsPrimary   
 FROM PRIMARYRECONSILEDATA WHERE TemplateId in ( '+CAST(@TEMPLATEID AS VARCHAR(10)) +', '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10)) +') 
 and Amount <> 0 
 and ID not in (select PrimaryId from  RECONSILERAWDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +'   
 And CAST(LASTUPDATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''')  
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''    
    
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY          
 END    
   
   
 SET @@SQLQUERYFORRECORDCOUNT = 'SELECT  @PrimaryRecord = COUNT(*) OVER () FROM PRIMARYRECONSILEDATA WHERE TemplateId in ( '+CAST(@TEMPLATEID AS VARCHAR(10)) +', '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10)) +')    
 and ID not in (select PrimaryId from  RECONSILERAWDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +'   
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''')  
 And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''    
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERYFORRECORDCOUNT = @@SQLQUERYFORRECORDCOUNT +' AND ' + @CONDITIONQUERY          
 END     
     
 EXEC sp_executesql @@SQLQUERYFORRECORDCOUNT, N'@PrimaryRecord int out', @PrimaryRecordCount out   
      
        
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '           SET @@SQLQUERY = @@SQLQUERY +' SELECT Id,RRN,AMOUNT,[DATE],'+CAST(@TEMPLATEID AS VARCHAR(10)) +'   TemplateId,'''+ @TEMPALTENAME +''' TemplateName,IsReconsiled  IsReconsile,ReconsileType,ReconsileDesc,
'''+CAST(@@COUNT + 3 AS VARCHAR(11)) +'''  TemplateSpilt,CAST(''False'' AS BIT) IsPrimary   
 FROM RECONSILERAWDATA WHERE ISRECONSILED = 0   and Amount <> 0  AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +' And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''        
      
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY          
 END   
   
   
 SET @@SQLQUERYFORRECORDCOUNT = 'SELECT  @NonPrimaryRecord = COUNT(*) OVER () FROM RECONSILERAWDATA WHERE ISRECONSILED = 0  and Amount <> 0   
 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10)) +' And CAST(DATE AS DATE) BETWEEN ''' + CAST(@FROMDATE AS  VARCHAR(11)) + ''' and ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''        
 IF(@CONDITIONQUERY != '')          
 BEGIN          
  SET @@SQLQUERYFORRECORDCOUNT = @@SQLQUERYFORRECORDCOUNT +' AND ' + @CONDITIONQUERY          
 END     
     
 EXEC sp_executesql @@SQLQUERYFORRECORDCOUNT, N'@NonPrimaryRecord int out', @NonPrimaryRecordCount out   
   
 IF(@NonPrimaryRecordCount =0)  
 BEGIN  
  SET @@SQLQUERYFORRECORDDELETE = @@SQLQUERYFORRECORDDELETE + ' DELETE FROM ##TEMPRECONSILEREPORT WHERE TemplateSpilt = '''+CAST(@@COUNT + 2 AS VARCHAR(11)) +''''  
 END  
  
 IF(@PRIMARYRECORDCOUNT > @NONPRIMARYRECORDCOUNT)  
 BEGIN  
  if(@NONPRIMARYRECORDCOUNT !=0)  
  BEGIN  
   SET @RecordCountDIFF = @PRIMARYRECORDCOUNT - @NONPRIMARYRECORDCOUNT  
   --WHILE (@intFlag <=@RecordCountDIFF)  
   -- BEGIN  
   --  SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '          
   --  SET @@SQLQUERY = @@SQLQUERY +' SELECT 0,'''',0,GETDATE(), '''','''','''','''','''','''+CAST(@@COUNT + 2 AS VARCHAR(11)) +''' TemplateSpilt,CAST(''True'' AS BIT) IsPrimary '    
   --  SET @intFlag = @intFlag + 1  
   -- END  
  END  
 END       
END          
SET @@COUNT = @@COUNT + 1          
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME,@CONDITIONQUERY          
END            
CLOSE RECONSILEREPORT_CURSOR            
DEALLOCATE RECONSILEREPORT_CURSOR           
        
      
exec(@@SQLQUERY)          
EXEC(@@SQLQUERYFORRECORDDELETE)  
          
SELECT Id,[RRNNUMBER],AMOUNT,[DATE],[TemplateId],[TemplateName],[IsReconsile],[ReconsileType],[ReconsileDesc],TemplateSpilt,IsPrimary  FROM ##TEMPRECONSILEREPORT         
DROP TABLE ##TEMPRECONSILEREPORT    
    
END