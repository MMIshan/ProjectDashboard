CREATE TABLE [dbo].[CommonData] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]        INT            NOT NULL,
    [WikiPageLink]     NVARCHAR (MAX) NULL,
    [ConfluencePageId] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.CommonData] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CommonData_dbo.ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

