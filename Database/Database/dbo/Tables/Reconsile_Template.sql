CREATE TABLE [dbo].[Reconsile_Template] (
    [Reconsile_Template_Id]  INT            IDENTITY (1, 1) NOT NULL,
    [Template_Name]          NVARCHAR (MAX) NOT NULL,
    [Source_Folder_Path]     NVARCHAR (MAX) NOT NULL,
    [Source_File_Extention]  NVARCHAR (15)  NOT NULL,
    [Source_Completion_Path] NVARCHAR (MAX) NOT NULL,
    [Source_Substring_Value] NVARCHAR (MAX) NOT NULL,
    [Source_Delimiter]       NVARCHAR (MAX) NOT NULL,
    [Source_HasHeader]       BIT            DEFAULT ((0)) NOT NULL,
    [Is_Active]              BIT            DEFAULT ((1)) NOT NULL,
    [LastUpdate]             DATETIME2 (7)  NULL,
    [CreatedDate]            DATETIME2 (7)  NULL,
    [Number_Of_Parameter]    INT            NULL,
    [Template_Group_id]      INT            NULL,
    [Template_Group_Status]  BIT            NULL, 
    [IsPrimary] BIT NOT NULL DEFAULT ((0)), 
    [ConditionQuery] VARCHAR(MAX) NULL, 
    [AmoutWithDecimal] BIT NULL 
);


GO

                CREATE TRIGGER [tr_Listener_1]
                ON dbo.[Reconsile_Template]
                AFTER INSERT, UPDATE, DELETE 
                AS
                SET NOCOUNT ON;
                --Trigger Reconsile_Template is rising...
                IF EXISTS (SELECT * FROM sys.services WHERE name = 'ListenerService_1')
                BEGIN
                    DECLARE @message NVARCHAR(MAX)
                    SET @message = N'<root/>'
                    IF ( EXISTS(SELECT 1))
                    BEGIN
                        DECLARE @retvalOUT NVARCHAR(MAX)
                        SET @retvalOUT = (SELECT [Reconsile_Template_Id],[Template_Name],[Source_Folder_Path],[Source_File_Extention],[Source_Completion_Path],[Source_Substring_Value],[Source_Delimiter],[Source_HasHeader],[Is_Active],[LastUpdate],[CreatedDate],[Number_Of_Parameter],[Template_Group_id],[Template_Group_Status] 
                                                     FROM INSERTED 
                                                     FOR XML PATH('row'), ROOT ('inserted'))
                        IF (@retvalOUT IS NOT NULL)
                        BEGIN SET @message = N'<root>' + @retvalOUT END                        
                        SET @retvalOUT = (SELECT [Reconsile_Template_Id],[Template_Name],[Source_Folder_Path],[Source_File_Extention],[Source_Completion_Path],[Source_Substring_Value],[Source_Delimiter],[Source_HasHeader],[Is_Active],[LastUpdate],[CreatedDate],[Number_Of_Parameter],[Template_Group_id],[Template_Group_Status] 
                                                     FROM DELETED 
                                                     FOR XML PATH('row'), ROOT ('deleted'))
                        IF (@retvalOUT IS NOT NULL)
                        BEGIN
                            IF (@message = N'<root/>') BEGIN SET @message = N'<root>' + @retvalOUT END
                            ELSE BEGIN SET @message = @message + @retvalOUT END
                        END 
                        IF (@message != N'<root/>') BEGIN SET @message = @message + N'</root>' END
                    END
                	--Beginning of dialog...
                	DECLARE @ConvHandle UNIQUEIDENTIFIER
                	--Determine the Initiator Service, Target Service and the Contract 
                	BEGIN DIALOG @ConvHandle 
                        FROM SERVICE [ListenerService_1] TO SERVICE 'ListenerService_1' ON CONTRACT [DEFAULT] WITH ENCRYPTION=OFF, LIFETIME = 60; 
	                --Send the Message
	                SEND ON CONVERSATION @ConvHandle MESSAGE TYPE [DEFAULT] (@message);
	                --End conversation
	                END CONVERSATION @ConvHandle;
                END
            
GO



CREATE TRIGGER [DBO].[UPDATETEMPLATE]
ON [DBO].[RECONSILE_TEMPLATE]
AFTER UPDATE, INSERT, DELETE
AS


IF EXISTS(SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
BEGIN
	UPDATE DBO.[RECONSILE_TEMPLATE] SET LASTUPDATE=GETDATE() 
	FROM INSERTED I
	WHERE DBO.[RECONSILE_TEMPLATE].RECONSILE_TEMPLATE_ID = I.RECONSILE_TEMPLATE_ID
END

IF EXISTS (SELECT * FROM INSERTED) AND NOT EXISTS(SELECT * FROM DELETED)
BEGIN
    UPDATE DBO.[RECONSILE_TEMPLATE] SET CREATEDDATE=GETDATE() 
	FROM INSERTED I
	WHERE DBO.[RECONSILE_TEMPLATE].RECONSILE_TEMPLATE_ID = I.RECONSILE_TEMPLATE_ID
END


            
