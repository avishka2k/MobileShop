CREATE TABLE [dbo].[CartItems] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [OrderId]   INT            NOT NULL,
    [ProductId] INT            NOT NULL,
    [Color]     NVARCHAR (MAX) NOT NULL,
    [Quantity]  INT            NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CartItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CartItems_OrderId]
    ON [dbo].[CartItems]([OrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CartItems_ProductId]
    ON [dbo].[CartItems]([ProductId] ASC);

