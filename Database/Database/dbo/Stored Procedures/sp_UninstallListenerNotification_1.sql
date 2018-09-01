
                        CREATE PROCEDURE dbo.sp_UninstallListenerNotification_1
                        AS
                        BEGIN
                            -- Notification Trigger drop statement.
                            
                IF OBJECT_ID ('dbo.tr_Listener_1', 'TR') IS NOT NULL
                    DROP TRIGGER dbo.[tr_Listener_1];
            
                            -- Service Broker uninstall statement.
                            
                DECLARE @serviceId INT
                SELECT @serviceId = service_id FROM sys.services 
                WHERE sys.services.name = 'ListenerService_1'
                DECLARE @ConvHandle uniqueidentifier
                DECLARE Conv CURSOR FOR
                SELECT CEP.conversation_handle FROM sys.conversation_endpoints CEP
                WHERE CEP.service_id = @serviceId AND ([state] != 'CD' OR [lifetime] > GETDATE() + 1)
                OPEN Conv;
                FETCH NEXT FROM Conv INTO @ConvHandle;
                WHILE (@@FETCH_STATUS = 0) BEGIN
    	            END CONVERSATION @ConvHandle WITH CLEANUP;
                    FETCH NEXT FROM Conv INTO @ConvHandle;
                END
                CLOSE Conv;
                DEALLOCATE Conv;
                -- Droping service and queue.
                DROP SERVICE [ListenerService_1];
                IF OBJECT_ID ('dbo.ListenerQueue_1', 'SQ') IS NOT NULL
	                DROP QUEUE dbo.[ListenerQueue_1];
            
                            IF OBJECT_ID ('dbo.sp_InstallListenerNotification_1', 'P') IS NOT NULL
                                DROP PROCEDURE dbo.sp_InstallListenerNotification_1
                            
                            DROP PROCEDURE dbo.sp_UninstallListenerNotification_1
                        END
                        