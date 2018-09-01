
                        CREATE PROCEDURE dbo.sp_InstallListenerNotification_1
                        AS
                        BEGIN
                            -- Service Broker configuration statement.
                            
                -- Setup Service Broker
                IF EXISTS (SELECT * FROM sys.databases 
                                    WHERE name = 'NPSL' AND (is_broker_enabled = 0 OR is_trustworthy_on = 0)) 
                BEGIN
                    ALTER DATABASE [NPSL] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                    ALTER DATABASE [NPSL] SET ENABLE_BROKER; 
                    ALTER DATABASE [NPSL] SET MULTI_USER WITH ROLLBACK IMMEDIATE
                    -- FOR SQL Express
                    ALTER AUTHORIZATION ON DATABASE::[NPSL] TO [sa]
                    ALTER DATABASE [NPSL] SET TRUSTWORTHY ON;
                END
                -- Create a queue which will hold the tracked information 
                IF NOT EXISTS (SELECT * FROM sys.service_queues WHERE name = 'ListenerQueue_1')
	                CREATE QUEUE dbo.[ListenerQueue_1]
                -- Create a service on which tracked information will be sent 
                IF NOT EXISTS(SELECT * FROM sys.services WHERE name = 'ListenerService_1')
	                CREATE SERVICE [ListenerService_1] ON QUEUE dbo.[ListenerQueue_1] ([DEFAULT]) 
            
                            -- Notification Trigger check statement.
                            
                IF OBJECT_ID ('dbo.tr_Listener_1', 'TR') IS NOT NULL
                    RETURN;
            
                            -- Notification Trigger configuration statement.
                            DECLARE @triggerStatement NVARCHAR(MAX)
                            DECLARE @select NVARCHAR(MAX)
                            DECLARE @sqlInserted NVARCHAR(MAX)
                            DECLARE @sqlDeleted NVARCHAR(MAX)
                            
                            SET @triggerStatement = N'
                CREATE TRIGGER [tr_Listener_1]
                ON dbo.[Reconsile_Template]
                AFTER INSERT, UPDATE, DELETE 
                AS
                SET NOCOUNT ON;
                --Trigger Reconsile_Template is rising...
                IF EXISTS (SELECT * FROM sys.services WHERE name = ''ListenerService_1'')
                BEGIN
                    DECLARE @message NVARCHAR(MAX)
                    SET @message = N''<root/>''
                    IF ( EXISTS(SELECT 1))
                    BEGIN
                        DECLARE @retvalOUT NVARCHAR(MAX)
                        %inserted_select_statement%
                        IF (@retvalOUT IS NOT NULL)
                        BEGIN SET @message = N''<root>'' + @retvalOUT END                        
                        %deleted_select_statement%
                        IF (@retvalOUT IS NOT NULL)
                        BEGIN
                            IF (@message = N''<root/>'') BEGIN SET @message = N''<root>'' + @retvalOUT END
                            ELSE BEGIN SET @message = @message + @retvalOUT END
                        END 
                        IF (@message != N''<root/>'') BEGIN SET @message = @message + N''</root>'' END
                    END
                	--Beginning of dialog...
                	DECLARE @ConvHandle UNIQUEIDENTIFIER
                	--Determine the Initiator Service, Target Service and the Contract 
                	BEGIN DIALOG @ConvHandle 
                        FROM SERVICE [ListenerService_1] TO SERVICE ''ListenerService_1'' ON CONTRACT [DEFAULT] WITH ENCRYPTION=OFF, LIFETIME = 60; 
	                --Send the Message
	                SEND ON CONVERSATION @ConvHandle MESSAGE TYPE [DEFAULT] (@message);
	                --End conversation
	                END CONVERSATION @ConvHandle;
                END
            '
                            
                            SET @select = STUFF((SELECT ',' + '[' + COLUMN_NAME + ']'
						                         FROM INFORMATION_SCHEMA.COLUMNS
						                         WHERE DATA_TYPE NOT IN  ('text','ntext','image','geometry','geography') AND TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Reconsile_Template' AND TABLE_CATALOG = 'NPSL'
						                         FOR XML PATH ('')
						                         ), 1, 1, '')
                            SET @sqlInserted = 
                                N'SET @retvalOUT = (SELECT ' + @select + N' 
                                                     FROM INSERTED 
                                                     FOR XML PATH(''row''), ROOT (''inserted''))'
                            SET @sqlDeleted = 
                                N'SET @retvalOUT = (SELECT ' + @select + N' 
                                                     FROM DELETED 
                                                     FOR XML PATH(''row''), ROOT (''deleted''))'                            
                            SET @triggerStatement = REPLACE(@triggerStatement
                                                     , '%inserted_select_statement%', @sqlInserted)
                            SET @triggerStatement = REPLACE(@triggerStatement
                                                     , '%deleted_select_statement%', @sqlDeleted)
                            EXEC sp_executesql @triggerStatement
                        END
                        