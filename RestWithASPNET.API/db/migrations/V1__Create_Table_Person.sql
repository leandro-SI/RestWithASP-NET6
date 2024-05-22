IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Persons]') AND type in (N'U')
)
BEGIN
    CREATE TABLE Persons (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] VARCHAR(80) NOT NULL,
        Gender VARCHAR(10) NOT NULL
    );
END