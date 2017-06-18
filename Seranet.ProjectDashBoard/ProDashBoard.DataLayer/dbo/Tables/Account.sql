CREATE TABLE [dbo].[Account] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AccountName]  NVARCHAR (MAX) NOT NULL,
    [AccCode]      NVARCHAR (MAX) NULL,
    [Availability] BIT            NOT NULL,
    [AccountOwner] INT            DEFAULT ((0)) NOT NULL,
    [Description]  VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Account_dbo.AccountOwner] FOREIGN KEY ([AccountOwner]) REFERENCES [dbo].[TeamMembers] ([Id])
);

