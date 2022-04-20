-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateCustomerInfo]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateCustomerInfo];
GO
CREATE PROCEDURE [dbo].[AddUpdateCustomerInfo]  
@Id INT=NULL,   
@UserId NVARCHAR(128), 
@FirstName NVARCHAR(255), 
@LastName NVARCHAR(255),   
@MiddleName NVARCHAR(255),  
@ContactNumber NVARCHAR(15),  
@Email NVARCHAR(255),   
@UserName NVARCHAR(50), 
@Password NVARCHAR(50), 
@Address NVARCHAR(1000)=NULL,  
@Landmark NVARCHAR(1000)=NULL,  
@CountryId INT=NULL,  
@CityId  INT=NULL,   
@LocationId  INT=NULL,    
@CreatedDate DATETIME,  
@UpdatedDate DATETIME,  
@DeviceToken NVARCHAR(2000)=NULL,
@StatusCode INT OUTPUT,  
@StatusMessage NVARCHAR(MAX) OUTPUT  
AS  
-- =============================================  
-- Author:  Harshitha
-- Created date: 17-09-2019 
-- Description: Add or Update CustomerInfo
-- Sample Call: 
    
/*
declare @StatusCode int,@StatusMessage NVARCHAR(100)
EXEC dbo.[AddUpdateCustomerInfo] 0, '9badb58e-046b-4c24-b7ae-e0ee4c195c2b', 'bharat', 'p', '', 9848022338, 'bharat@gmail.com', 'raji', 'bharat123', 1,'1'  
 null,null, '2017-01-06 12:11:01.307',  '2017-01-06 12:11:01.307', @StatusCode OUTPUT, @StatusMessage OUTPUT 
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
   SAVE TRANSACTION AddUpdateCustomerInfo;  
       
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
		 INSERT INTO dbo.UserInfo(UserId, FirstName, Lastname, MiddleName, ContactNumber, UserName, [Password], RoleId, Email, [Address],
		 Landmark,Country,City,Location,          IsActive,  CreatedDate,  UpdatedDate,AccessToken)   
           VALUES (@UserId, @FirstName, @Lastname, @MiddleName, @ContactNumber, @UserName, @Password, 4, @Email,@Address,@Landmark,  
@CountryId,   @CityId ,@LocationId , 1,  @CreatedDate,  @UpdatedDate,@DeviceToken)  
     
        SET @StatusCode = SCOPE_IDENTITY()  
           SET @StatusMessage='Customer Registered Successfully'
		   
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
	    UPDATE Users SET UserName=@UserName where Id=@UserId
		UPDATE dbo.UserInfo SET FirstName=@FirstName, Lastname=@Lastname, MiddleName=@MiddleName, ContactNumber=@ContactNumber, 
		  Password=@Password,  Email=@Email, [Address]= @Address,Landmark= @Landmark,  
          Country=@CountryId,UserName=@UserName,   City=@CityId ,Location=@LocationId ,UpdatedDate=@UpdatedDate,AccessToken=@DeviceToken WHERE Id=@Id 
    	  
		SET @StatusCode = @Id     
		SET @StatusMessage='Customer Info Updated Successfully'  
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
ROLLBACK TRANSACTION AddUpdateCustomerInfo;  
SET @StatusMessage = ERROR_MESSAGE()  
SET @StatusCode=0  
RAISERROR ('AddUpdateCustomerInfo: %d: %s', 16, 1, @error, @message);  
END CATCH  
END 
