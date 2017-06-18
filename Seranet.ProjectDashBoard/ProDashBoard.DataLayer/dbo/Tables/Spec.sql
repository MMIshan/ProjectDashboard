CREATE TABLE [dbo].[Spec] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AccountId]    INT            NOT NULL,
    [linkId]       NVARCHAR (MAX) NULL,
    [SpecLevel]    INT            NULL,
    [PendingCount] INT            NULL,
    CONSTRAINT [PK_dbo.Spec] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Spec_dbo.AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id])
);

