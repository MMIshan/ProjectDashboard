CREATE TABLE [dbo].[FinancialSummary] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [AccountId ]      INT             NULL,
    [AccountName ]    VARCHAR (MAX)   NULL,
    [Year ]           INT             NULL,
    [Month ]          INT             NULL,
    [MonthName]       VARCHAR (MAX)   NULL,
    [Quarter ]        INT             NULL,
    [ExpectedHours  ] DECIMAL (18, 2) NULL,
    [ActualHours  ]   DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

