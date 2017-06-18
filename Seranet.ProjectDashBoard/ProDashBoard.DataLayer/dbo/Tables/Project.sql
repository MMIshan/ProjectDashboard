CREATE TABLE [dbo].[Project] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [AccountId]    INT             NULL,
    [Name]         NVARCHAR (MAX)  NULL,
    [ProjetCode]   NVARCHAR (MAX)  NULL,
    [Enabled]      BIT             NOT NULL,
    [RowVersion]   VARBINARY (MAX) NULL,
    [ProjectOwner] NVARCHAR (MAX)  NULL,
    [Description]  VARCHAR (MAX)   NULL,
    [Billable]     BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Project] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Project_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id])
);

