CREATE TABLE [dbo].[UserDetails] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [User]     NVARCHAR (50)  NULL,
    [PassWord] NVARCHAR (MAX) NULL,
    [Name]     NVARCHAR (MAX) NULL,
    [Email]    NVARCHAR (50)  NULL,
    [Address]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserDetails_UserDetails] FOREIGN KEY ([Id]) REFERENCES [dbo].[UserDetails] ([Id])
);