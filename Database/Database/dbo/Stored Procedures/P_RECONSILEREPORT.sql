﻿CREATE PROCEDURE [P_RECONSILEREPORT]  
@GROUPTEMPLATEID INT
AS  
BEGIN  
DECLARE @PRIMARYTEMPLATEID INT,
@PRIMARYTEMPLATENAME VARCHAR(30),
@@COUNT INT = 0,
@@SQLQUERY VARCHAR(MAX),
@TEMPLATEID INT,
@TEMPALTENAME VARCHAR(100)


SELECT @PRIMARYTEMPLATEID = RECONSILE_TEMPLATE_ID,@PRIMARYTEMPLATENAME = TEMPLATE_NAME  FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID AND ISPRIMARY =1

SET @@SQLQUERY = 'CREATE TABLE ##TEMPRECONSILEREPORT([DETAIL RRN] NVARCHAR(MAX),AMOUNT NUMERIC(18,2),[DATE] VARCHAR(400))'
EXEC(@@SQLQUERY)

SET @@SQLQUERY = '' 
DECLARE RECONSILEREPORT_CURSOR CURSOR  
LOCAL  FORWARD_ONLY  FOR  
SELECT DISTINCT TEMPLATEID,RT.TEMPLATE_NAME
FROM PRIMARYRECONSILEDATA  
JOIN RECONSILE_TEMPLATE RT ON RECONSILE_TEMPLATE_ID = TEMPLATEID
WHERE TEMPLATEID IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID AND ISPRIMARY <>1 )
OPEN RECONSILEREPORT_CURSOR  
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME
WHILE @@FETCH_STATUS = 0  
BEGIN  
IF @@COUNT = 0 
BEGIN
	SET @@SQLQUERY = 'INSERT INTO ##TEMPRECONSILEREPORT SELECT ''RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT ''NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
END
ELSE
BEGIN
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT ''RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],SUM(AMOUNT) AS [AMOUNT],'''' AS [DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT ''NON RECONSILED FOR TEMPLATE : ' + @TEMPALTENAME + ''' AS [DETAIL RRN],SUM(AMOUNT) AS [AMOUNT],'''' AS [DATE] FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM RECONSILERAWDATA WHERE ISRECONSILED = 0 AND TEMPLATEID = '+CAST(@TEMPLATEID AS VARCHAR(10))
END
SET @@COUNT = @@COUNT + 1
FETCH NEXT FROM RECONSILEREPORT_CURSOR INTO  @TEMPLATEID,@TEMPALTENAME
END  
CLOSE RECONSILEREPORT_CURSOR  
DEALLOCATE RECONSILEREPORT_CURSOR 
END

	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT ''NON RECONSILED FOR PRIMARY TEMPLATE : ' + @PRIMARYTEMPLATENAME + ''' AS [DETAIL RRN],ISNULL(SUM(AMOUNT),0) AS [AMOUNT],'''' AS [DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10))
	SET @@SQLQUERY = @@SQLQUERY +' UNION ALL '
	SET @@SQLQUERY = @@SQLQUERY +' SELECT RRN,AMOUNT,[DATE] FROM PRIMARYRECONSILEDATA WHERE TEMPLATEID = '+CAST(@PRIMARYTEMPLATEID AS VARCHAR(10))


	EXEC(@@SQLQUERY)

	SET @@SQLQUERY = '' 
	SET @@SQLQUERY = @@SQLQUERY + ' UPDATE ##TEMPRECONSILEREPORT SET [DATE] = '''' WHERE [DATE]= ''1900-01-01'''
	EXEC(@@SQLQUERY)

SELECT [DETAIL RRN],AMOUNT,[DATE]  FROM ##TEMPRECONSILEREPORT
DROP TABLE ##TEMPRECONSILEREPORT