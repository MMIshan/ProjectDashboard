CREATE TABLE [dbo].[Summary] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [ProjectID] INT             NULL,
    [Year]      INT             NULL,
    [Quarter]   INT             NULL,
    [Rating]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_dbo.Summary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Summary_dbo.ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Account] ([Id])
);

