﻿--EXEC P_INSERTRECONSILEDATA 'F:\Sachine_Prj\ReconsileSPTest\sdpgkjl3.fjz.txt',6, 5
--EXEC P_INSERTRECONSILEDATA 'F:\SACHINE_PRJ\MOVEPATH\MOVETOCOMPLETE\4COLUMN.TXT',4 


CREATE PROCEDURE [dbo].[P_INSERTRECONSILEDATA]  
@FILEPATH NVARCHAR(MAX),
@NUMBEROFCOLUMNS INT,
@TEMPLATEID INT
AS  
BEGIN  

DECLARE @DYNAMICTABLE NVARCHAR(MAX),@DYNAMICJOIN NVARCHAR(MAX) ='',@DYNAMICINSERT NVARCHAR(MAX) ='',@DYNAMICINSERTVALUE NVARCHAR(MAX) ='', @COLUMNCOUNT INT,@TEMPLATE_ID INT = @TEMPLATEID,@ISPRIMARY INT,@TEMPLATEGROUPID INT

SET @NUMBEROFCOLUMNS = @NUMBEROFCOLUMNS - 3
SELECT @ISPRIMARY = ISPRIMARY,@TEMPLATEGROUPID = TEMPLATE_GROUP_ID FROM RECONSILE_TEMPLATE WHERE RECONSILE_TEMPLATE_ID = @TEMPLATEID
SET @DYNAMICTABLE = 'CREATE TABLE ##TEMPRECONSILEDATE(RRN NVARCHAR(MAX),[DATE] VARCHAR(400),AMOUNT NUMERIC(18,2)'
SET @COLUMNCOUNT =1
WHILE @COLUMNCOUNT <= @NUMBEROFCOLUMNS
BEGIN
      SET @DYNAMICTABLE = @DYNAMICTABLE + ',RECOCOL' + CAST(@COLUMNCOUNT + 3 AS VARCHAR(10))+ ' VARCHAR(MAX) '
	  SET @DYNAMICJOIN = @DYNAMICJOIN + ' AND TARGET.RECOCOL'+ CAST(@COLUMNCOUNT + 3 AS VARCHAR(10)) + ' = SOURCE.RECOCOL'+ CAST(@COLUMNCOUNT + 3 AS VARCHAR(10))
	  SET @DYNAMICINSERT = @DYNAMICINSERT + ',RECOCOL'+ CAST(@COLUMNCOUNT + 3 AS VARCHAR(10))
	  SET @DYNAMICINSERTVALUE = @DYNAMICINSERTVALUE + ',SOURCE.RECOCOL'+ CAST(@COLUMNCOUNT + 3 AS VARCHAR(10))
      SET @COLUMNCOUNT = @COLUMNCOUNT + 1
END
SET @DYNAMICTABLE = @DYNAMICTABLE + ') '
EXEC(@DYNAMICTABLE);

SET @DYNAMICTABLE = '' 
SET @DYNAMICTABLE = 'ALTER TABLE  ##TEMPRECONSILEDATE ADD FILENAME NVARCHAR(300)'
EXEC(@DYNAMICTABLE);

SET @DYNAMICTABLE = ''
SET @DYNAMICTABLE = ' BULK INSERT ##TEMPRECONSILEDATE FROM ''' + @FILEPATH + ''' WITH (FIELDTERMINATOR = '','',ROWTERMINATOR = ''0X0A'') '
EXEC(@DYNAMICTABLE);

UPDATE ##TEMPRECONSILEDATE SET [DATE] =
CASE WHEN [DATE] = '' OR [DATE] IS NULL THEN  CONVERT(DATE,CONVERT([char](10),getdate(),(103)),105)
WHEN ISDATE([DATE]) <> 1 THEN CONVERT(CHAR(10),(CONVERT(DATE,CAST([DATE]  AS DATETIME),103)),105) ELSE  [DATE] END

UPDATE ##TEMPRECONSILEDATE SET [AMOUNT] = CONVERT(decimal(10, 2), [AMOUNT]/ 100.00) 


IF(@ISPRIMARY =1)
BEGIN
	SET @DYNAMICTABLE =''
	SET @DYNAMICTABLE = 'MERGE PRIMARYRECONSILEDATA AS TARGET  
	USING ##TEMPRECONSILEDATE AS SOURCE   
	ON (TARGET.RRN = SOURCE.RRN AND TARGET.[DATE] = SOURCE.[DATE] AND TARGET.AMOUNT = SOURCE.AMOUNT AND TARGET.FILENAME = SOURCE.FILENAME ' + @DYNAMICJOIN
	SET @DYNAMICTABLE = @DYNAMICTABLE + ') WHEN NOT MATCHED BY TARGET THEN INSERT (RRN, [DATE], AMOUNT,FILENAME,TEMPLATEID,ISPRIMARY' + @DYNAMICINSERT
	SET @DYNAMICTABLE = @DYNAMICTABLE + ') VALUES (SOURCE.RRN, CAST(SOURCE.[DATE] AS DATE), SOURCE.AMOUNT,SOURCE.FILENAME,' +CAST(@TEMPLATE_ID AS VARCHAR(10))  + ',' +CAST(@ISPRIMARY AS VARCHAR(1)) + @DYNAMICINSERTVALUE 
	SET @DYNAMICTABLE = @DYNAMICTABLE + ');'  
	EXEC(@DYNAMICTABLE)
END
ELSE
BEGIN
	SET @DYNAMICTABLE =''
	SET @DYNAMICTABLE = 'MERGE RECONSILERAWDATA AS TARGET  
	USING ##TEMPRECONSILEDATE AS SOURCE   
	ON (TARGET.RRN = SOURCE.RRN AND TARGET.[DATE] = SOURCE.[DATE] AND TARGET.AMOUNT = SOURCE.AMOUNT AND TARGET.FILENAME = SOURCE.FILENAME ' + @DYNAMICJOIN
	SET @DYNAMICTABLE = @DYNAMICTABLE + ') WHEN NOT MATCHED BY TARGET THEN INSERT (RRN, [DATE], AMOUNT,FILENAME,TEMPLATEID,ISPRIMARY' + @DYNAMICINSERT
	SET @DYNAMICTABLE = @DYNAMICTABLE + ') VALUES (SOURCE.RRN, CAST(SOURCE.[DATE] AS DATE), SOURCE.AMOUNT,SOURCE.FILENAME,' +CAST(@TEMPLATE_ID AS VARCHAR(10))  + ',' +CAST(@ISPRIMARY AS VARCHAR(1)) + @DYNAMICINSERTVALUE 
	SET @DYNAMICTABLE = @DYNAMICTABLE + ');'  
	EXEC(@DYNAMICTABLE)
END
DELETE FROM ##TEMPRECONSILEDATE

--SET @DYNAMICTABLE ='' 
--SET @DYNAMICTABLE = 'INSERT INTO ##TEMPRECONSILEDATE(RRN,[DATE],AMOUNT'  + @DYNAMICINSERT  
--SET @DYNAMICTABLE = @DYNAMICTABLE + ') SELECT RRN, [DATE], AMOUNT'  + @DYNAMICINSERT + ' FROM RECONSILERAWDATA WHERE ISRECONSILED =0 '  
--SET @DYNAMICTABLE = @DYNAMICTABLE + ' GROUP BY  RRN, [DATE], AMOUNT'+ @DYNAMICINSERT    
--SET @DYNAMICTABLE = @DYNAMICTABLE + ' HAVING COUNT(1) > 1' 
--EXEC(@DYNAMICTABLE) 

SET @DYNAMICTABLE ='' 
SET @DYNAMICTABLE = 'INSERT INTO ##TEMPRECONSILEDATE(RRN,[DATE],AMOUNT'  + @DYNAMICINSERT  
SET @DYNAMICTABLE = @DYNAMICTABLE + ') SELECT SOURCE.RRN, CAST(SOURCE.[DATE] AS DATE) [DATE], SOURCE.AMOUNT' + @DYNAMICINSERTVALUE + ' FROM PRIMARYRECONSILEDATA SOURCE JOIN RECONSILERAWDATA TARGET ON'
SET @DYNAMICTABLE = @DYNAMICTABLE + ' TARGET.RRN = SOURCE.RRN AND TARGET.[DATE] = SOURCE.[DATE] AND TARGET.AMOUNT = SOURCE.AMOUNT' + @DYNAMICJOIN
SET @DYNAMICTABLE = @DYNAMICTABLE + ' AND TARGET.ISRECONSILED =0 AND SOURCE.ISRECONSILED =0 AND SOURCE.TEMPLATEID IN (SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE Template_Group_id = ' + CAST(@TEMPLATEGROUPID AS VARCHAR(10)) +' )'  
EXEC(@DYNAMICTABLE) 




SET @DYNAMICTABLE =''   
SET @DYNAMICTABLE = 'MERGE RECONSILEDDATA AS TARGET  
USING ##TEMPRECONSILEDATE AS SOURCE   
ON (TARGET.RRN = SOURCE.RRN AND TARGET.[DATE] = SOURCE.[DATE] AND TARGET.AMOUNT = SOURCE.AMOUNT' + @DYNAMICJOIN
SET @DYNAMICTABLE = @DYNAMICTABLE + ') WHEN NOT MATCHED BY TARGET THEN INSERT (RRN, [DATE], AMOUNT,TEMPLATEID' + @DYNAMICINSERT  
SET @DYNAMICTABLE = @DYNAMICTABLE + ') VALUES (SOURCE.RRN, SOURCE.[DATE], SOURCE.AMOUNT,'+CAST(@TEMPLATE_ID AS VARCHAR(10))  + @DYNAMICINSERTVALUE 
SET @DYNAMICTABLE = @DYNAMICTABLE + ');'  
EXEC(@DYNAMICTABLE) 
 
DROP TABLE ##TEMPRECONSILEDATE 

UPDATE RefreshDashBoard SET ReconsileUpdate = 1;
END