CREATE TABLE [dbo].[Products] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (MAX) NOT NULL,
    [Description]   NVARCHAR (MAX) NOT NULL,
    [Reviews]       NVARCHAR (MAX) NOT NULL,
    [ReviewScore]   NVARCHAR (MAX) NOT NULL,
    [Price]         NVARCHAR (MAX) NOT NULL,
    [Stock]         INT            NOT NULL,
    [Delivery]      NVARCHAR (MAX) NOT NULL,
    [SKU]           NVARCHAR (MAX) NOT NULL,
    [CategoryId]    INT            NOT NULL,
    [BrandId]       INT            NOT NULL,
    [FirstImageUrl] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Products_BrandId]
    ON [dbo].[Products]([BrandId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId]
    ON [dbo].[Products]([CategoryId] ASC);

