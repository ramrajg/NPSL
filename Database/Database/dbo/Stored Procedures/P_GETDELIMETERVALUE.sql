--EXEC P_GETDELIMETERVALUE '2'      
CREATE PROCEDURE P_GETDELIMETERVALUE   
@DELIMITERID INT=0        
AS        
BEGIN        
      
      
SELECT  Delimiter_Id DelimiterId,     
        Delimiter_Value  DelimiterValue     
       FROM [Delimiter]       
        WHERE Delimiter_Id = @DELIMITERID        
          OR  (ISNULL(@DELIMITERID, 0) = 0)      
      
END 