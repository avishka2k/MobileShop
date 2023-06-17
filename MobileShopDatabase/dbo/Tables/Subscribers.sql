CREATE TABLE [dbo].[Subscribers] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Email]    NVARCHAR (MAX) NOT NULL,
    [DateTime] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Subscribers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

