CREATE TABLE [dbo].[ProcessComplianceResults] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [AccountID]          INT           NULL,
    [ProjectID]          INT           NULL,
    [Year]               INT           NULL,
    [Quarter]            INT           NULL,
    [QualityParameterId] INT           NULL,
    [Rating]             NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.ProcessComplianceResults] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProcessComplianceResults_dbo.AccountID] FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.ProcessComplianceResults_dbo.ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_dbo.ProcessComplianceResults_dbo.QualityParameterId] FOREIGN KEY ([QualityParameterId]) REFERENCES [dbo].[ProcessComplianceQualityParameters] ([Id])
);

