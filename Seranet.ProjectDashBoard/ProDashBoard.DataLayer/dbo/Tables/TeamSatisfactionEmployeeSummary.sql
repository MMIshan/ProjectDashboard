CREATE TABLE [dbo].[TeamSatisfactionEmployeeSummary] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [EmpId]     INT             NULL,
    [AccountId] INT             NULL,
    [Year]      INT             NULL,
    [Quarter]   INT             NULL,
    [Rating]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_dbo.TeamSatisfactionEmployeeSummary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TeamSatisfactionEmployeeSummary_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.TeamSatisfactionEmployeeSummary_dbo.EmpId] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[TeamMembers] ([Id])
);

