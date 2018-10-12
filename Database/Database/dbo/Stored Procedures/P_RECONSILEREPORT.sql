﻿CREATE PROCEDURE [P_ReconsileReport]  
@GROUPTEMPLATEID INT    
AS      
BEGIN      
DECLARE @PRIMARYTEMPLATEID INT,    
@PRIMARYTEMPLATENAME VARCHAR(30),    
@@COUNT INT = 0,    
@@SQLQUERY VARCHAR(MAX),    
@TEMPLATEID INT,    
@TEMPALTENAME VARCHAR(100),    
@CONDITIONQUERY VARCHAR(MAX)    
    
    
SELECT @PRIMARYTEMPLATEID = RECONSILE_TEMPLATE_ID,@PRIMARYTEMPLATENAME = TEMPLATE_NAME  FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID AND ISPRIMARY =1    
    
SET @@SQLQUERY = 'CREATE TABLE ##TEMPRECONSILEREPORT([DETAIL RRN] NVARCHAR(MAX),AMOUNT NUMERIC(18,2),[DATE] VARCHAR(400))'    
EXEC(@@SQLQUERY)    
    
SET @@SQLQUERY = ''     
DECLARE RECONSILEREPORT_CURSOR CURSOR      
LOCAL  FORWARD_ONLY  FOR      
SELECT DISTINCT TEMPLATEID,RT.TEMPLATE_NAME,ISNULL(CONDITIONQUERY,'') AS CONDITIONQUERY    
FROM PRIMARYRECONSILEDATA      
JOIN RECONSILE_TEMPLATE RT ON RECONSILE_TEMPLATE_ID = TEMPLATEID    
WHERE TEMPLATEID IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID )    
OPEN RECONSILEREPORT_CURSOR      
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME,@CONDITIONQUERY    
WHILE @@FETCH_STATUS = 0      
BEGIN      
IF @@COUNT = 0     
BEGIN    
 SET @@SQLQUERY = 'INSERT INTO ##TEMPRECONSILEREPORT SELECT ''RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))    
   
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''PRIMARY NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'    
   
 IF(@CONDITIONQUERY != '')    
 BEGIN    
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY    
 END    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'    
 IF(@CONDITIONQUERY != '')    
 BEGIN    
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY    
 END    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'     
END    
ELSE    
BEGIN    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''----------------------------------------'' AS [DETAIL RRN],0 AS [AMOUNT],'''' AS [DATE]'   
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],SUM(AMOUNT) AS [AMOUNT],'''' AS [DATE]'   
 SET @@SQLQUERY = @@SQLQUERY +' FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED =1 AND  TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED =1 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''PRIMARY NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],SUM(AMOUNT) AS [AMOUNT],'''' AS [DATE] FROM PRIMARYRECONSILEDATA WHERE '  
 SET @@SQLQUERY = @@SQLQUERY +' ISRECONSILED <> 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'    
  
 IF(@CONDITIONQUERY != '')    
 BEGIN    
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY    
 END    
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'    
 IF(@CONDITIONQUERY != '')    
 BEGIN    
  SET @@SQLQUERY = @@SQLQUERY +' AND ' + @CONDITIONQUERY    
 END   
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT ''NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'     
 SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '    
 SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] '  
 SET @@SQLQUERY = @@SQLQUERY +' FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID  IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = ' + CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +')'     
END    
SET @@COUNT = @@COUNT + 1    
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME,@CONDITIONQUERY    
END      
CLOSE RECONSILEREPORT_CURSOR      
DEALLOCATE RECONSILEREPORT_CURSOR     
END    
    
 EXEC(@@SQLQUERY)    
    
 SET @@SQLQUERY = ''     
 SET @@SQLQUERY = @@SQLQUERY + ' UPDATE ##TEMPRECONSILEREPORT SET [DATE] = '''' WHERE [DATE]= ''1900-01-01'''    
 EXEC(@@SQLQUERY)    
    
SELECT [DETAIL RRN],AMOUNT,[DATE]  FROM ##TEMPRECONSILEREPORT    
DROP TABLE ##TEMPRECONSILEREPORT


