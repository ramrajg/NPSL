CREATE TYPE [dbo].[udt_template] AS TABLE (
    [TemplateId]           INT            NOT NULL,
    [TemplateName]         NVARCHAR (MAX) NOT NULL,
    [SourceFolder]         NVARCHAR (MAX) NOT NULL,
    [SourceExtention]      NVARCHAR (MAX) NULL,
    [SourceCompletionPath] NVARCHAR (MAX) NOT NULL,
    [SourceSubstringValue] NVARCHAR (MAX) NULL,
    [SourceDelimiter]      NVARCHAR (MAX) NOT NULL,
    [SourceHasHeader]      BIT            NULL,
    [NumberOfParameters]   INT            NOT NULL,
    [TemplateGroupId]      INT            NULL,
    [TemplateGroupStatus]  BIT            NOT NULL,
    [IsActive]             BIT            NULL,
	[IsPrimary]			   BIT            NULL,
	[ConditionQuery]	   VARCHAR(MAX)   NULL,
	[AmoutWithDecimal] [bit] NULL);

