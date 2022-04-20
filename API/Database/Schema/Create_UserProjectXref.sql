--UserProjectXref
IF OBJECT_ID ('dbo.UserProjectXref', 'U') IS NOT NULL
    DROP TABLE dbo.[UserProjectXref];
GO

 CREATE Table UserProjectXref(
 UserId INT FOREIGN KEY REFERENCES UserInfo(Id) NOT NULL,
 ProjectId INT FOREIGN KEY REFERENCES Project(Id) NOT NULL
 )