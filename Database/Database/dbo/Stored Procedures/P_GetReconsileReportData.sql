

--EXEC P_GetReconsileReportData 0,'Aug 10 2018','Aug 13 2018'  --used reconsile Process    
CREATE  PROCEDURE [dbo].[P_GetReconsileReportData]            
@GROUPTEMPLATEID INT=0,  
@SELECTEDTEMPLATEID INT,        
@ReconsileTypeId int =0,     
@FROMDATE DATETIME,        
@TODATE DATETIME          
AS              
BEGIN              
  
DECLARE @@SQLQUERY VARCHAR(MAX)  
        
IF OBJECT_ID('tempdb..##TEMPRECONSILEREPORT') IS NOT NULL DROP TABLE ##TEMPRECONSILEREPORT;  
CREATE TABLE ##TEMPRECONSILEREPORT(  
 PrimaryRRNNumber VARCHAR(400),  
 PrimaryAmount NUMERIC(18,2),  
 PrimaryDATE Datetime,  
 PrimaryTemplateId  int,  
 PrimaryTemplateName VARCHAR(400),  
 PrimaryIsReconsile bit,  
  
 PrimaryReconsileType VARCHAR(400),  
 PrimaryReconsileDesc VARCHAR(400),  
 NonPrimaryRRNNumber VARCHAR(400),  
 NonPrimaryAmount NUMERIC(18,2),  
 NonPrimaryDate Datetime,  
 NonPrimaryTemplateId  int,  
 NonPrimaryTemplateName VARCHAR(400),  
 NonPrimaryIsReconsile bit,  
 NonPrimaryReconsileType VARCHAR(400),  
 NonPrimaryReconsileDesc VARCHAR(400),  
 denseCount int,  
 SumAmount  NUMERIC(18,2),  
 TotalTrx int  
 )      
  
      
 SET @@SQLQUERY = ' insert into ##TEMPRECONSILEREPORT       
 SELECT RD.RRN PrimaryRRNNumber,      
 RD.AMOUNT PrimaryAmount,      
 CAST(RD.[DATE] AS DATE) AS [PrimaryDATE],      
 RD.TEMPLATEID PrimaryTemplateId,       
 RTT.TEMPLATE_NAME PrimaryTemplateName,       
 CAST(''TRUE'' AS BIT) PrimaryIsReconsile,      
 RD.RECONSILETYPE PrimaryReconsileType,      
 RD.RECONSILEDESC PrimaryReconsileDesc,      
 RRD.RRN NonPrimaryRRNNumber,      
 RRD.AMOUNT NonPrimaryAmount,      
 CAST(RRD.[DATE] AS DATE) AS [NonPrimaryDATE],      
 RRD.TEMPLATEID NonPrimaryTemplateId,       
 NRTT.TEMPLATE_NAME NonPrimaryTemplateName,       
 CAST(''TRUE'' AS BIT) NonPrimaryIsReconsile,      
 RRD.RECONSILETYPE NonPrimaryReconsileType,      
 RRD.RECONSILEDESC NonPrimaryReconsileDesc,      
 dense_Rank() OVER (ORDER BY RRD.TEMPLATEID ASC) denseCount  
 ,0 as SumAmount,  
 0 as TotalTrx  
 FROM PRIMARYRECONSILEDATA RD       
 JOIN RECONSILERAWDATA RRD on RRD.PRIMARYID = RD.ID       
 JOIN RECONSILE_TEMPLATE RTT ON RD.TEMPLATEID = RTT.RECONSILE_TEMPLATE_ID        
 JOIN RECONSILE_TEMPLATE NRTT ON RRD.TEMPLATEID = NRTT.RECONSILE_TEMPLATE_ID        
 WHERE CAST(RRD.[DATE] AS DATE) BETWEEN  ''' + CAST(@FROMDATE AS  VARCHAR(11)) + '''  AND ''' + CAST(@TODATE AS  VARCHAR(11)) + ''''  
 IF(@RECONSILETYPEID=0)  
 BEGIN  
  SET @@SQLQUERY = @@SQLQUERY + ' AND RRD.RECONSILETYPE =''A'''  
 END  
 ELSE IF (@RECONSILETYPEID = 1 )  
 BEGIN  
  SET @@SQLQUERY = @@SQLQUERY + ' AND RRD.RECONSILETYPE =''M'''  
 END  
 ELSE IF (@RECONSILETYPEID = 2)  
 BEGIN  
  SET @@SQLQUERY = @@SQLQUERY + ' AND RRD.RECONSILETYPE IN (''A'',''M'')'  
 END  
   
  
 IF(@GROUPTEMPLATEID !=0)  
 BEGIN  
 SET @@SQLQUERY = @@SQLQUERY + ' AND RTT.TEMPLATE_GROUP_ID = '+CAST(@GROUPTEMPLATEID AS VARCHAR(10)) +' '  
 END   
  
 SET @@SQLQUERY = @@SQLQUERY + ' AND RTT.IS_ACTIVE =1 AND  RRD.TEMPLATEID = '+CAST(@SELECTEDTEMPLATEID AS VARCHAR(10)) +' '  
 EXEC(@@SQLQUERY)  
  
  
 UPDATE ##TEMPRECONSILEREPORT SET SUMAMOUNT = TOTALAMOUNT,TOTALTRX = TOTALTRXCOUNT  
 FROM ##TEMPRECONSILEREPORT INNER JOIN   
  (SELECT SUM(NONPRIMARYAMOUNT) AS TOTALAMOUNT, COUNT(DENSECOUNT) AS TOTALTRXCOUNT,DENSECOUNT FROM ##TEMPRECONSILEREPORT GROUP BY DENSECOUNT) AGG ON ##TEMPRECONSILEREPORT.DENSECOUNT = AGG.DENSECOUNT  
  
SELECT * FROM ##TEMPRECONSILEREPORT  
END