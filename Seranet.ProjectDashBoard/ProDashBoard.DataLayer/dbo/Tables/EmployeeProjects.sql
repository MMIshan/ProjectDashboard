CREATE TABLE [dbo].[EmployeeProjects] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [EmpId]         INT NULL,
    [AccountId]     INT NULL,
    [ProjectId]     INT NULL,
    [Availability]  BIT NULL,
    [BillableHours] INT NULL,
    [Lead]          BIT NULL,
    [Billable]      INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.EmployeeProjects] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EmployeeProjects_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.EmployeeProjects_dbo.EmpId] FOREIGN KEY ([EmpId]) REFERENCES [dbo].[TeamMembers] ([Id]),
    CONSTRAINT [FK_dbo.EmployeeProjects_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

