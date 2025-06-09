DROP TABLE [dbo].[Serilogs];
GO

CREATE TABLE [dbo].[Serilogs] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Message]       NVARCHAR (MAX) NULL,           -- Increased to MAX for longer messages
    [MessageTemplate] NVARCHAR (MAX) NULL,         -- Template for structured logging
    [Level]         NVARCHAR (50)  NULL,
    [TimeStamp]     DATETIME2      NOT NULL DEFAULT GETDATE(), -- Use DATETIME2 for precision
    [Exception]     NVARCHAR (MAX) NULL,
    [Properties]    NVARCHAR (MAX) NULL,           -- For structured log data
    [CorrelationId] NVARCHAR (36)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO