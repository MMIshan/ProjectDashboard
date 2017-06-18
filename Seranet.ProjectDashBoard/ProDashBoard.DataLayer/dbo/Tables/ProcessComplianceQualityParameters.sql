CREATE TABLE [dbo].[ProcessComplianceQualityParameters] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [QualityParameter] NVARCHAR (MAX) NULL,
    [Status]           BIT            NULL,
    [ParameterOrder]   INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

