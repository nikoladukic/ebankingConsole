CREATE TABLE [dbo].[User] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (MAX) NOT NULL,
    [LastName]  NVARCHAR (MAX) NOT NULL,
    [Email]     VARCHAR (256) NOT NULL,
    [Password]  VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

CREATE TABLE [dbo].[Currency] (
    [Id]   INT           NOT NULL,
    [Name] VARCHAR (256) NOT NULL,
    [Code] VARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[Account] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [Balance]    DECIMAL (18, 2) NOT NULL,
    [Status]     INT             NOT NULL,
    [Number]     VARCHAR (256)   NOT NULL,
    [UserId]     INT             NOT NULL,
    [CurrencyId] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Number] ASC),
    CONSTRAINT [FK_Account_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Account_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([Id])
);

CREATE TABLE [dbo].[Transaction] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Amount]        DECIMAL (18, 2) NOT NULL,
    [Date]          DATETIME2 (0)   NOT NULL,
    [FromAccountId] INT             NULL,
    [ToAccountId]   INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transaction_FromAccount] FOREIGN KEY ([FromAccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_Transaction_ToAccount] FOREIGN KEY ([ToAccountId]) REFERENCES [dbo].[Account] ([Id])
);
