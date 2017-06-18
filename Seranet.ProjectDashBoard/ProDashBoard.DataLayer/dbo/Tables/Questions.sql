CREATE TABLE [dbo].[Questions] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [QuestionName]  NVARCHAR (MAX) NOT NULL,
    [QuestionType]  NVARCHAR (MAX) NOT NULL,
    [Availability]  BIT            NOT NULL,
    [CalcExist]     BIT            NOT NULL,
    [QuestionOrder] INT            NULL,
    [Mandatory]     BIT            NULL,
    [Comment]       BIT            NULL,
    [MaxValue]      INT            NULL,
    [QuestionNote]  NVARCHAR (MAX) DEFAULT (NULL) NULL,
    [QuestionHint]  NVARCHAR (MAX) DEFAULT (NULL) NULL,
    CONSTRAINT [PK_dbo.Questions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

