CREATE TABLE [dbo].[Invoices] (
    [ID] [int] NOT NULL IDENTITY,
    [customerID] [nvarchar](max),
    [name] [nvarchar](max),
    [date] [datetime] NOT NULL,
    [paid] [bit] NOT NULL,
    [PDFFileID] [int] NOT NULL,
    [customer_userName] [nvarchar](128),
    CONSTRAINT [PK_dbo.Invoices] PRIMARY KEY ([ID])
)
CREATE INDEX [IX_PDFFileID] ON [dbo].[Invoices]([PDFFileID])
CREATE INDEX [IX_customer_userName] ON [dbo].[Invoices]([customer_userName])
CREATE TABLE [dbo].[UserAccount] (
    [userName] [nvarchar](128) NOT NULL,
    [hashPassword] [nvarchar](max),
    [permissionGroup] [nvarchar](max),
    CONSTRAINT [PK_dbo.UserAccount] PRIMARY KEY ([userName])
)
CREATE TABLE [dbo].[PDFFile] (
    [ID] [int] NOT NULL IDENTITY,
    [Content] [varbinary](max),
    CONSTRAINT [PK_dbo.PDFFile] PRIMARY KEY ([ID])
)
ALTER TABLE [dbo].[Invoices] ADD CONSTRAINT [FK_dbo.Invoices_dbo.UserAccount_customer_userName] FOREIGN KEY ([customer_userName]) REFERENCES [dbo].[UserAccount] ([userName])
ALTER TABLE [dbo].[Invoices] ADD CONSTRAINT [FK_dbo.Invoices_dbo.PDFFile_PDFFileID] FOREIGN KEY ([PDFFileID]) REFERENCES [dbo].[PDFFile] ([ID]) ON DELETE CASCADE
