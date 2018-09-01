

--EXEC P_GetReconsileReportData 0,'Aug 10 2018','Aug 13 2018'  --used reconsile Process    
CREATE PROCEDURE [dbo].[P_GetReconsileReportData]      
@GroupTemplateId INT=0,
@FromDate DateTime,
@ToDate DateTime  
AS      
BEGIN      

CREATE TABLE #Reconsile_Template 
(  
    Template_Id int
)  

insert into #Reconsile_Template
select Reconsile_Template_Id from Reconsile_Template where Template_Group_id = @GroupTemplateId  OR  (ISNULL(@GroupTemplateId, 0) = 0)    and Is_Active =1


select RRN RRNNumber,Amount,CAST([Date] AS Date) as [Date],RD.TEMPLATEID TemplateId, RTT.Template_Name TemplateName, CAST('True' AS bit) IsReconsile from RECONSILEDDATA RD
join  #Reconsile_Template RT on RD.TEMPLATEID = RT.Template_Id
join Reconsile_Template RTT on RD.TEMPLATEID = RTT.Reconsile_Template_Id
where CAST(RD.LastUpdate AS DATE) between @FromDate and @ToDate
union 


select RRN RRNNumber,Amount,CAST([Date] AS Date) as [Date],RD.TEMPLATEID TemplateId, RTT.Template_Name TemplateName,RD.ISRECONSILED IsReconsile from RECONSILERAWDATA RD
join  #Reconsile_Template RT on RD.TEMPLATEID = RT.Template_Id
join Reconsile_Template RTT on RD.TEMPLATEID = RTT.Reconsile_Template_Id
where RD.ISRECONSILED =0 
and CAST(RD.LastUpdate AS DATE) between @FromDate and @ToDate

END   

