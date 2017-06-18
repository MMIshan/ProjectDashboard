CREATE TABLE [dbo].[CustomerSatisfactionSummary] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [AccountId] INT             NULL,
    [ProjectId] INT             NULL,
    [Year]      INT             NULL,
    [Quarter]   INT             NULL,
    [Rating]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_dbo.CustomerSatisfactionSummary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CustomerSatisfactionSummary_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.CustomerSatisfactionSummary_dbo.ProjectID] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

