--ActivityRight
IF OBJECT_ID ('Notification', 'U') IS NOT NULL
    DROP TABLE [Notification];
GO	
CREATE TABLE [Notification](
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
OrderId INT  NULL  FOREIGN KEY REFERENCES dbo.[Order](Id),
UserId INT NOT NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
[Desc] NVARCHAR(MAX),
[IsRead] BIT default 0,
NotificationTypeId INT  NOT NULL  FOREIGN KEY REFERENCES dbo.[TypecdDmt](TypeCdId),
IsActive BIT NOT NULL DEFAULT 1,
CreatedByUserId	INT NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
CreatedDate	DATETIME  NOT NULL,
UpdatedByUserId INT  NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
UpdatedDate DATETIME  NOT NULL
)