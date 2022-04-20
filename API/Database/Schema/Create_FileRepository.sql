--FileRepository
IF OBJECT_ID ('dbo.FileRepository', 'U') IS NOT NULL
    DROP TABLE dbo.FileRepository;
GO
CREATE TABLE dbo.FileRepository(
Id INT IDENTITY PRIMARY KEY NOT NULL,
ProjectId INT FOREIGN KEY REFERENCES dbo.Project(Id) NOT NULL, 
FileName VARCHAR(100) NOT NULL,
FileLocation VARCHAR(250) NOT NULL,
FileExtension VARCHAR(25) NOT NULL,
IsActive BIT NOT NULL,
CreatedByUserId INT FOREIGN KEY REFERENCES dbo.[UserInfo](Id) NOT NULL ,
CreatedDate DATETIME NOT NULL,
UpdatedByUserId INT FOREIGN KEY REFERENCES dbo.[UserInfo](Id) NOT NULL,
UpdatedDate DATETIME NOT NULL 
)

ALTER Table  FileRepository ADD  VideoFileName NVARCHAR(1000)  NULL
update FileRepository set VideoFileName='Videos'
ALTER Table  FileRepository ALTER COLUMN  VideoFileName NVARCHAR(1000) NOT NULL