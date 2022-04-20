-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateLocation]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateLocation];
GO
CREATE PROCEDURE [dbo].[AddUpdateLocation]
@Id INT = NULL,
@Name1 NVARCHAR(500),
@Name2 NVARCHAR(500),
@Code VARCHAR(50),
@CityId int,
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
-- Author:		<Harshitha>
-- Create date: <19-03-2020>
-- Description:	<Add or Update Location>
-- Sample Call: EXEC AddUpdateLocation null,'Guntur','Guntur','Gun',1,1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
  
 BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AddUpdateLocation ;
	  IF ISNULL(@Id,0) = 0
		  BEGIN
			   IF EXISTS (SELECT 1 FROM dbo.[Location] WHERE (Code=@Code OR Name1= @Name1))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Location Name or Code Already Exists '
				   END
			   ELSE
				   BEGIN
						INSERT INTO dbo.[Location] (Name1,Name2,Code,CityId, IsActive, CreatedByUserId, CreatedDate, UpdatedByUserId, UpdatedDate) 
						 VALUES (@Name1,@Name2,@Code,@CityId,  @IsActive, @CreatedByUserId, @CreatedDate, @UpdatedByUserId, @UpdatedDate)

						SET @StatusCode = SCOPE_IDENTITY()											
						
						SET @StatusMessage='Location Added Successfully'
				   END
		  END
	  ELSE
		  BEGIN
				IF EXISTS (SELECT 1 FROM dbo.[Location] WHERE Id != @Id AND (Code=@Code OR Name1= @Name1))
					BEGIN			   
						SET @StatusCode = -1
						SET @StatusMessage='Location Name or Code Already Exists '
					END
				ELSE
					BEGIN
						 UPDATE dbo.[Location] SET Name1=@Name1, Name2=@Name2,Code=@Code,CityId=@CityId,IsActive=@IsActive,UpdatedByUserId=@UpdatedByUserId, UpdatedDate=@UpdatedDate WHERE Id=@Id				 
											
						SET @StatusCode = @Id
						SET @StatusMessage='Location Updated Successfully'
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
		ROLLBACK TRANSACTION AddUpdateLocation ;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AddUpdateLocation : %d: %s', 16, 1, @error, @message);
 END CATCH
END


