-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateCountry]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateCountry];
GO
CREATE PROCEDURE [dbo].[AddUpdateCountry]
@Id INT = NULL,
@Name1 NVARCHAR(500),
@Name2 NVARCHAR(500),
@Code VARCHAR(50),
@IsActive BIT = 1,
@CreatedByUserId INT,
@CreatedDate DATETIME,
@UpdatedByUserId INT,
@UpdatedDate DATETIME,
@StatusCode INT OUTPUT,
@StatusMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
-- =============================================
-- Author:		<Bhavani>
-- Create date: <19-03-2020>
-- Description:	<Add or Update Country>
-- Sample Call: EXEC AddUpdateCountry null,'United States','United States','US',1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
  
 BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AddUpdateCountry ;
	  IF ISNULL(@Id,0) = 0
		  BEGIN
			   IF EXISTS (SELECT 1 FROM dbo.[Country] WHERE (Code=@Code OR Name1= @Name1 OR Name2= @Name2))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Country Name or Code Already Exists '
				   END
			   ELSE
				   BEGIN
						INSERT INTO dbo.[Country] (Name1,Name2,Code, IsActive, CreatedByUserId, CreatedDate, UpdatedByUserId, UpdatedDate) 
						 VALUES (@Name1,@Name2,@Code, @IsActive, @CreatedByUserId, @CreatedDate, @UpdatedByUserId, @UpdatedDate)
						SET @StatusCode = SCOPE_IDENTITY()											
						
						SET @StatusMessage='Country Added Successfully'
				   END
		  END
	  ELSE
		  BEGIN
				IF EXISTS (SELECT 1 FROM dbo.[Country] WHERE Id != @Id AND (Code=@Code OR Name1= @Name1 OR Name2= @Name2))
					BEGIN			   
						SET @StatusCode = -1
						SET @StatusMessage='Country Name or Code Already Exists '
					END
				ELSE
					BEGIN
						 UPDATE dbo.[Country] SET Name1=@Name1, Name2=@Name2,Code=@Code,IsActive=@IsActive,UpdatedByUserId=@UpdatedByUserId, UpdatedDate=@UpdatedDate WHERE Id=@Id				 
											
						SET @StatusCode = @Id
						SET @StatusMessage='Country Updated Successfully'
					END
		  END
		IF @trancount = 0
            COMMIT;
 END TRY
 BEGIN CATCH
	DECLARE @error int, @message NVARCHAR(4000), @xstate int;
	select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
	if @xstate = -1
		ROLLBACK;
	if @xstate = 1 and @trancount = 0
		ROLLBACK
	if @xstate = 1 and @trancount > 0
		ROLLBACK TRANSACTION AddUpdateCountry ;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AddUpdateCountry : %d: %s', 16, 1, @error, @message);
 END CATCH
END
