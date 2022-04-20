-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateUserInfo]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateUserInfo];
GO
CREATE PROCEDURE [dbo].[AddUpdateUserInfo]  
@Id INT=NULL,  
@UserId NVARCHAR(128),
@FirstName NVARCHAR(255),  
@LastName NVARCHAR(255),  
@MiddleName NVARCHAR(255),  
@ContactNumber NVARCHAR(15), 
@Email NVARCHAR(255),  
@UserName NVARCHAR(50),  
@Password NVARCHAR(50),  
@RoleId INT, 
@ProjectIds VARCHAR(255),  
@ManagerId INT,  
@Address NVARCHAR(250),   
@IsActive BIT,  
@CreatedByUserId INT,  
@CreatedDate DATETIME,  
@UpdatedByUserId INT,  
@UpdatedDate DATETIME,  
@StatusCode INT OUTPUT,  
@StatusMessage NVARCHAR(MAX) OUTPUT  
AS  
-- =============================================  
-- Author:  Harshitha
-- Created date: 17-09-2019 
-- Description: Add or Update UserInfo
-- Sample Call: 
    
/*
declare @StatusCode int,@StatusMessage NVARCHAR(100)
EXEC dbo.AddUpdateUserInfo 0, '9badb58e-046b-4c24-b7ae-e0ee4c195c2b', 'bharat', 'p', '', 9848022338, 'bharat@gmail.com', 'raji', 'bharat123', 1,'1'  
null, '', '', '2,3',null,1,1, '2017-01-06 12:11:01.307',1, '2017-01-06 12:11:01.307', @StatusCode OUTPUT, @StatusMessage OUTPUT 
select StatusCode=@StatusCode ,StatusMessage=@StatusMessage
  		  
*/ 
-- =============================================  
BEGIN  
SET NOCOUNT ON;  
DECLARE @trancount INT;  
SET @trancount = @@trancount;  
        
BEGIN TRY  
IF @trancount = 0  
  BEGIN TRANSACTION  
ELSE  
   SAVE TRANSACTION AddUpdateUserInfo;  
       
IF ISNULL(@Id,0) = 0  
  BEGIN  
    IF EXISTS (SELECT 1 FROM dbo.UserInfo WHERE UserName=@UserName)  
		BEGIN  
		SET @StatusCode = -1  
		SET @StatusMessage='User Name already exists'  
		END 
	ELSE IF EXISTS (SELECT 1 FROM dbo.UserInfo WHERE ContactNumber=@ContactNumber)  
		BEGIN  
		SET @StatusCode = -1  
		SET @StatusMessage='Contact Number already exists'  
		END 
		ELSE
		BEGIN
		 INSERT INTO dbo.UserInfo(UserId, FirstName, Lastname, MiddleName, ContactNumber, UserName, [Password], RoleId, Email, ManagerId,  
          [Address], IsActive, CreatedByUserId, CreatedDate, UpdatedByUserId, UpdatedDate)   
           VALUES (@UserId, @FirstName, @Lastname, @MiddleName, @ContactNumber, @UserName, @Password, @RoleId, @Email, @ManagerId, @Address,   
          @IsActive, @CreatedByUserId, @CreatedDate, @UpdatedByUserId, @UpdatedDate )  
     
        SET @StatusCode = SCOPE_IDENTITY()  
           SET @StatusMessage='User Added Successfully'
		   INSERT INTO  [dbo].[UserProjectXref]
					(UserId,[ProjectId])
					SELECT @StatusCode, Item FROM dbo.SplitString(@ProjectIds,',')
      END
  END  
ELSE  
	BEGIN  
		IF EXISTS (SELECT 1 FROM dbo.UserInfo WHERE Id!=@Id AND UserName=@UserName)  
		BEGIN  
		SET @StatusCode = -1  
		SET @StatusMessage='User Name already exists'  
	END  
ELSE IF EXISTS (SELECT 1 FROM dbo.UserInfo WHERE Id!=@Id AND ContactNumber=@ContactNumber)  
	BEGIN  
		SET @StatusCode = -1  
		SET @StatusMessage='Contact Number already exists'  
		END 
  	 
ELSE  
	BEGIN  
	 UPDATE Users set UserName=@UserName where Id=@UserId
		UPDATE dbo.UserInfo SET FirstName=@FirstName, Lastname=@Lastname, MiddleName=@MiddleName, ContactNumber=@ContactNumber,ManagerId=@ManagerId, 
		Address=@Address, Password=@Password, RoleId=@RoleId,UserName=@UserName, Email=@Email, IsActive=@IsActive, UpdatedByUserId=@UpdatedByUserId, UpdatedDate=@UpdatedDate WHERE Id=@Id 
    	 	DELETE FROM  [dbo].[UserProjectXref] WHERE UserId=@Id
			  INSERT INTO  [dbo].[UserProjectXref]
					([UserId],[ProjectId])
					SELECT @Id, Item FROM dbo.SplitString(@ProjectIds,',')
		SET @StatusCode = @Id     
		SET @StatusMessage='User Updated Successfully'  
	END  
END 
   
IF @trancount = 0  
COMMIT;  
END TRY  
BEGIN CATCH  
DECLARE @error int, @message NVARCHAR(4000), @xstate int;  
SELECT @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();  
IF @xstate = -1  
ROLLBACK;  
IF @xstate = 1 AND @trancount = 0  
ROLLBACK;  
IF @xstate = 1 AND @trancount > 0  
ROLLBACK TRANSACTION AddUpdateUserInfo;  
SET @StatusMessage = ERROR_MESSAGE()  
SET @StatusCode=0  
RAISERROR ('AddUpdateUserInfo: %d: %s', 16, 1, @error, @message);  
END CATCH  
END 