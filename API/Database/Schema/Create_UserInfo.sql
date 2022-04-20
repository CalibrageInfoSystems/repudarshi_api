
--UserInfo
IF OBJECT_ID ('[UserInfo]', 'U') IS NOT NULL
    DROP TABLE dbo.[UserInfo];
GO	
CREATE TABLE [UserInfo](
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
UserId NVARCHAR(128) NOT NULL,
FirstName Varchar(255) NOT NULL,
Lastname Varchar(255) NOT NULL,
MiddleName Varchar(255),
ContactNumber Varchar(15) NOT NULL, 
UserName Varchar(50) NOT NULL,
[Password] Varchar(50) NOT NULL,
RoleId INT FOREIGN KEY REFERENCES dbo.ROLE(Id) NOT NULL,
Email Varchar(255),
ManagerId INT FOREIGN KEY REFERENCES dbo.[UserInfo](Id),  
IsActive bit NOT NULL DEFAULT 1,
CreatedByUserId	INT NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
CreatedDate	DATETIME  NOT NULL ,
UpdatedByUserId INT NULL  FOREIGN KEY REFERENCES dbo.[UserInfo](Id),
UpdatedDate DATETIME  NOT NULL
)
   ALTER TABLE UserInfo
ALTER COLUMN Address NVARCHAR(1000) NULL;
ALTER TABLE UserInfo
              ADD Landmark NVARCHAR(1000) NULL,
			  Country INT NULL FOREIGN KEY REFERENCES dbo.Country(Id),
			  City INT NULL FOREIGN KEY REFERENCES dbo.City(Id),
			  Location INT NULL FOREIGN KEY REFERENCES dbo.Location(Id)
ALTER TABLE UserInfo
ADD  AccessToken NVARCHAR(2000) NULL;