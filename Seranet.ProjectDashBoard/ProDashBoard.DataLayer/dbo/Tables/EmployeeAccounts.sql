CREATE TABLE [dbo].[EmployeeAccounts] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [EmpId]     INT NULL,
    [ProjectId] INT NULL,
    CONSTRAINT [PK_dbo.EmployeeAccounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EmployeeAccounts_dbo.EmpId] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[TeamMembers] ([Id]),
    CONSTRAINT [FK_dbo.EmployeeAccounts_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

