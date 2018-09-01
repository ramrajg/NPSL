CREATE TYPE [dbo].[udt_templateGroup] AS TABLE (
    [TemplateGroupId]       INT            NOT NULL,
    [TemplateGroupName]     NVARCHAR (MAX) NOT NULL,
    [IsTemplateGroupActive] BIT            NULL);

