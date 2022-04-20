--ActivityRight
IF OBJECT_ID ('ActivityRight', 'U') IS NOT NULL
    DROP TABLE ActivityRight;
GO	
CREATE TABLE ActivityRight(
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
Name NVarchar(255) UNIQUE NOT NULL,
[Desc] NVarchar(500),
IsActive bit NOT NULL DEFAULT 1,
CreatedByUserId	INT NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
CreatedDate	DATETIME  NOT NULL,
UpdatedByUserId INT  NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
UpdatedDate DATETIME  NOT NULL
)
GO