CREATE TABLE [dbo].[ProcessComplianceSummary] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [AccountId]      INT             NULL,
    [ProjectId]      INT             NULL,
    [Year]           INT             NULL,
    [Quarter]        INT             NULL,
    [Rating]         DECIMAL (18, 2) NULL,
    [ProcessVersion] NVARCHAR (MAX)  NULL,
    [Threshold]      DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProcessComplianceSummary_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.ProcessComplianceSummary_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

