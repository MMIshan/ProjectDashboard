CREATE TABLE [dbo].[FinancialResults] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [EmpId ]              INT             NULL,
    [EmpName ]            VARCHAR (MAX)   NULL,
    [AccountId ]          INT             NULL,
    [AccountName ]        VARCHAR (MAX)   NULL,
    [Year ]               INT             NULL,
    [Month ]              INT             NULL,
    [Quarter ]            INT             NULL,
    [BillableType ]       INT             NULL,
    [AllocatedHours ]     DECIMAL (18, 2) NULL,
    [BillableHours ]      DECIMAL (18, 2) NULL,
    [TotalReportedHours ] DECIMAL (18, 2) NULL,
    [ConsiderableHours]   DECIMAL (18, 2) NULL,
    [ExtraOrLag]          DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

