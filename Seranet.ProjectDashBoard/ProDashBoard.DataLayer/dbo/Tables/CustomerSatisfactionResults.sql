CREATE TABLE [dbo].[CustomerSatisfactionResults] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [AccountId]         INT            NULL,
    [ProjectId]         INT            NULL,
    [Year]              INT            NULL,
    [Quarter]           INT            NULL,
    [QuestionOrder]     INT            DEFAULT ((0)) NULL,
    [ActualQuestion]    NVARCHAR (MAX) NULL,
    [DashboardQuestion] NVARCHAR (MAX) NULL,
    [Answer]            NVARCHAR (MAX) NULL,
    [CalcExist]         BIT            NULL,
    CONSTRAINT [PK_dbo.CustomerSatisfactionResults] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CustomerSatisfactionResults_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.CustomerSatisfactionResults_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

