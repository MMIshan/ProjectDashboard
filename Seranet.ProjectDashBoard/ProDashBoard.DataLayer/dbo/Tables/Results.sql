CREATE TABLE [dbo].[Results] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [MemberId]   INT            NULL,
    [AccountID]  INT            NULL,
    [ProjectID]  INT            NULL,
    [Year]       INT            NULL,
    [Quarter]    INT            NULL,
    [QuestionID] INT            NULL,
    [Answer]     NVARCHAR (MAX) NULL,
    [Comment]    VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Results] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Results_dbo.AccountID] FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_dbo.Results_dbo.MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[TeamMembers] ([Id]),
    CONSTRAINT [FK_dbo.Results_dbo.ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_dbo.Results_dbo.QuestionID] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Questions] ([Id])
);

