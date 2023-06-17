CREATE TABLE [dbo].[Colors] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [ColorCode] NVARCHAR (MAX) NOT NULL,
    [ProductId] INT            NOT NULL,
    CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Colors_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Colors_ProductId]
    ON [dbo].[Colors]([ProductId] ASC);

