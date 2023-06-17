CREATE TABLE [dbo].[VisitCounts] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [VisitCount] INT NOT NULL,
    CONSTRAINT [PK_VisitCounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

