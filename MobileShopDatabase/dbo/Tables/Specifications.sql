CREATE TABLE [dbo].[Specifications] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [ProductId] INT            NOT NULL,
    CONSTRAINT [PK_Specifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Specifications_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Specifications_ProductId]
    ON [dbo].[Specifications]([ProductId] ASC);

