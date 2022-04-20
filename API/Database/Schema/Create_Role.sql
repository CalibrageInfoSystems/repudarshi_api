--Role
IF OBJECT_ID ('dbo.Role', 'U') IS NOT NULL
    DROP TABLE dbo.[Role];
GO
CREATE TABLE dbo.[Role](
 Id INT IDENTITY NOT NULL PRIMARY KEY,
 Code NVarchar(50) NOT NULL ,
 [Name] NVarchar(250) NOT NULL, 
 [Desc] NVarchar(500) NULL,
 ParentRoleId INT NULL,
 IsActive BIT NOT NULL,
 CreatedByUserId INT NULL ,
 CreatedDate DATETIME NOT NULL,
 UpdatedByUserId INT NULL,
 UpdatedByUserId DATETIME NOT NULL
)


 