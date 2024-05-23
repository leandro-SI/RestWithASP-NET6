IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U')
)
BEGIN
    CREATE TABLE Users (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserName VARCHAR(80) NOT NULL UNIQUE,
        FullName VARCHAR(120) NOT NULL,
        [Password] VARCHAR(130) NOT NULL DEFAULT '0',
        RefreshToken VARCHAR(500) NULL DEFAULT '0',
        RefreshTokenExpiryTime DATETIME DEFAULT NULL
    );
END