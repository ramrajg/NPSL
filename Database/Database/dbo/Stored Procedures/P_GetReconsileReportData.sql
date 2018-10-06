﻿

--EXEC P_GetReconsileReportData 0,'Aug 10 2018','Aug 13 2018'  --used reconsile Process    
CREATE  PROCEDURE [DBO].[P_GETRECONSILEREPORTDATA]        
@GROUPTEMPLATEID INT=0,  
@FROMDATE DATETIME,  
@TODATE DATETIME    
AS        
BEGIN        
  
CREATE TABLE #RECONSILE_TEMPLATE   
(    
    TEMPLATE_ID INT  
)    
  
INSERT INTO #RECONSILE_TEMPLATE  
SELECT RECONSILE_TEMPLATE_ID FROM RECONSILE_TEMPLATE WHERE TEMPLATE_GROUP_ID = @GROUPTEMPLATEID  OR  (ISNULL(@GROUPTEMPLATEID, 0) = 0)    AND IS_ACTIVE =1  
  
  
SELECT RRN RRNNUMBER,AMOUNT,CAST([DATE] AS DATE) AS [DATE],RD.TEMPLATEID TEMPLATEID, RTT.TEMPLATE_NAME TEMPLATENAME, CAST('TRUE' AS BIT) ISRECONSILE FROM PRIMARYRECONSILEDATA RD  
JOIN  #RECONSILE_TEMPLATE RT ON RD.TEMPLATEID = RT.TEMPLATE_ID  
JOIN RECONSILE_TEMPLATE RTT ON RD.TEMPLATEID = RTT.RECONSILE_TEMPLATE_ID  
WHERE CAST(RD.LASTUPDATE AS DATE) BETWEEN @FROMDATE AND @TODATE AND RD.ISRECONSILED =1  
UNION   
  
  
SELECT RRN RRNNUMBER,AMOUNT,CAST([DATE] AS DATE) AS [DATE],RD.TEMPLATEID TEMPLATEID, RTT.TEMPLATE_NAME TEMPLATENAME,RD.ISRECONSILED ISRECONSILE FROM RECONSILERAWDATA RD  
JOIN  #RECONSILE_TEMPLATE RT ON RD.TEMPLATEID = RT.TEMPLATE_ID  
JOIN RECONSILE_TEMPLATE RTT ON RD.TEMPLATEID = RTT.RECONSILE_TEMPLATE_ID  
WHERE RD.ISRECONSILED =0   
AND CAST(RD.LASTUPDATE AS DATE) BETWEEN @FROMDATE AND @TODATE  
  
END     
  
