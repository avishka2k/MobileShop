CREATE TABLE [dbo].[Orders] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TotalPrice]  FLOAT (53)     NOT NULL,
    [Delivery]    FLOAT (53)     NOT NULL,
    [DateAndTime] DATETIME2 (7)  NOT NULL,
    [FirstName]   NVARCHAR (MAX) NOT NULL,
    [LastName]    NVARCHAR (MAX) NOT NULL,
    [Email]       NVARCHAR (MAX) NOT NULL,
    [PhoneNumber] NVARCHAR (MAX) NOT NULL,
    [Address1]    NVARCHAR (MAX) NOT NULL,
    [Address2]    NVARCHAR (MAX) NOT NULL,
    [Country]     NVARCHAR (MAX) NOT NULL,
    [State]       NVARCHAR (MAX) NOT NULL,
    [ZipCode]     INT            NOT NULL,
    [Payment]     NVARCHAR (MAX) NOT NULL,
    [OrderStatus] INT            NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

