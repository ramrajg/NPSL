﻿
--EXEC P_GETTEMPLATE '0'  
CREATE PROCEDURE  [dbo].[P_ManualReconsileProcess]      
@pSelectedResult udt_manualReconsile Readonly

AS      
BEGIN      

DECLARE @DIFFAMOUNT numeric(18,2)
declare @sumAmountNonPrimary numeric(18,2) = (select sum(Amount) from RECONSILERAWDATA where id in (select Id from @pSelectedResult where [Type] IN('NP','T')))
declare @sumAmountPrimary numeric(18,2) = (select sum(Amount) from PRIMARYRECONSILEDATA where id in (select Id from @pSelectedResult where [Type] ='P'))
if(@sumAmountNonPrimary>@sumAmountPrimary)
begin
	SET @DIFFAMOUNT = @sumAmountNonPrimary - @sumAmountPrimary
	INSERT INTO PRIMARYRECONSILEDATA (RRN,DATE,AMOUNT,RECOCOL4,RECOCOL5,RECOCOL6,RECOCOL7,RECOCOL8,RECOCOL9,RECOCOL10,RECOCOL11,RECOCOL12,RECOCOL13,RECOCOL14,RECOCOL15,RECOCOL16,FILENAME,TEMPLATEID,ISRECONSILED,ISPRIMARY,RECONSILETYPE,RECONSILEDESC)
	SELECT RRN,GETDATE(),@DIFFAMOUNT,RECOCOL4,RECOCOL5,RECOCOL6,RECOCOL7,RECOCOL8,RECOCOL9,RECOCOL10,RECOCOL11,RECOCOL12,RECOCOL13,RECOCOL14,RECOCOL15,RECOCOL16,FILENAME,TEMPLATEID,0,1,'T',RECONSILEDESC FROM PRIMARYRECONSILEDATA WHERE ID  IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] ='P')
	UPDATE PRIMARYRECONSILEDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] ='P')
	UPDATE RECONSILERAWDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] IN('NP','T'))
end
else if (@sumAmountPrimary>@sumAmountNonPrimary)
begin
	SET @DIFFAMOUNT =@sumAmountPrimary - @sumAmountNonPrimary
	INSERT INTO RECONSILERAWDATA (RRN,DATE,AMOUNT,RECOCOL4,RECOCOL5,RECOCOL6,RECOCOL7,RECOCOL8,RECOCOL9,RECOCOL10,RECOCOL11,RECOCOL12,RECOCOL13,RECOCOL14,RECOCOL15,RECOCOL16,FILENAME,TEMPLATEID,ISRECONSILED,ISPRIMARY,RECONSILETYPE,RECONSILEDESC)
	SELECT RRN,GETDATE(),@DIFFAMOUNT,RECOCOL4,RECOCOL5,RECOCOL6,RECOCOL7,RECOCOL8,RECOCOL9,RECOCOL10,RECOCOL11,RECOCOL12,RECOCOL13,RECOCOL14,RECOCOL15,RECOCOL16,FILENAME,TEMPLATEID,0,0,'T',RECONSILEDESC FROM RECONSILERAWDATA WHERE ID  IN (SELECT TOP 1 ID FROM @pSelectedResult WHERE [TYPE] IN('NP','T'))
	UPDATE PRIMARYRECONSILEDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] ='P')
	UPDATE RECONSILERAWDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] IN('NP','T'))
end
else if (@sumAmountPrimary=@sumAmountNonPrimary)
begin
UPDATE PRIMARYRECONSILEDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] ='P')
UPDATE RECONSILERAWDATA SET ISRECONSILED = 1,RECONSILETYPE = 'M' WHERE ID IN (SELECT ID FROM @pSelectedResult WHERE [TYPE] IN('NP','T'))
end

END