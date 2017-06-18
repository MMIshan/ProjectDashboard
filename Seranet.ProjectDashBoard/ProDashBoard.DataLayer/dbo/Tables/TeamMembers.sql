CREATE TABLE [dbo].[TeamMembers] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [MemberName]   NVARCHAR (MAX) NOT NULL,
    [AdminRights]  BIT            DEFAULT ((0)) NOT NULL,
    [Availability] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.TeamMembers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

