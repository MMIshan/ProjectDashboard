CREATE TABLE [dbo].[Customer] (
    [Id]          INT           NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Address]     NVARCHAR (50) NULL,
    [City]        NVARCHAR (50) NULL,
    [Country]     NVARCHAR (50) NULL,
    [DateOfBirth] DATETIME      NULL,
    [Age]         INT           NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

