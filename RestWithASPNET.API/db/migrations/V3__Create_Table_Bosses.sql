IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Bosses]') AND type in (N'U')
)
BEGIN
    CREATE TABLE Bosses (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] VARCHAR(80) NOT NULL,
        Comment VARCHAR(200) NOT NULL
    );
END