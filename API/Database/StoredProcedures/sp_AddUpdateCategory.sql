-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateCategory]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateCategory];
GO
CREATE PROCEDURE [dbo].[AddUpdateCategory]
    @Id int = NULL,
	@Name1 nvarchar(500),
	@Name2 nvarchar(500),  
	@ParentCategoryId INT=NULL,
	@CategoryLevel INT=NULL,
	@FileName VARCHAR(250),
	@FileLocation VARCHAR(1000),
	@FileExtension VARCHAR(50),
	@IsActive bit,
	@CreatedByUserId int,
	@CreatedDate datetime,
	@UpdatedByUserId int,
	@UpdatedDate datetime,
	@StatusCode INT OUTPUT,
    @StatusMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
-- =============================================
-- Author:		<Harshitha>
-- Create date: <30-03-2020>
-- Description:	<Add or Update Category>
-- Sample Call: EXEC [AddUpdateCategory] 'SPAR','SPAR',NULL,NULL,NULL,NULL,NULL,1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
  BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AddUpdateCategory;
	  IF ISNULL(@Id,0) = 0
		          BEGIN	 
			   IF EXISTS (SELECT 1 FROM dbo.[Category] WHERE ( (Name1= @Name1 OR Name2=@Name2) AND ParentCategoryId=@ParentCategoryId))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Category Name is Already Exists.'
				   END
			   ELSE
				   BEGIN
				        INSERT INTO [dbo].[Category]
								   ([Name1]
								   ,[Name2]
								   ,[ParentCategoryId]
								   ,[CategoryLevel]
								   ,[FileLocation]
								   ,[FileName]
								   ,[FileExtention]
								   ,[IsActive]
								   ,[CreatedByUserId]
								   ,[CreatedDate]
								   ,[UpdatedByUserId]
								   ,[UpdatedDate])
						 VALUES (@Name1
						         ,@Name2
								,@ParentCategoryId
								,@CategoryLevel
	                             ,@FileLocation
								,@FileName 
								 ,@FileExtension
								 ,@IsActive
								 ,@CreatedByUserId
								 ,GETDATE()
								 ,@UpdatedByUserId
								 ,GETDATE()
								)
						SET @StatusCode = SCOPE_IDENTITY()	
						
						 
						SET @StatusMessage='Category Added Successfully'
			      END
		    END
	  ELSE
		        BEGIN		
				     IF EXISTS (SELECT 1 FROM dbo.[Category] WHERE Id != @Id AND ((Name1= @Name1 OR Name2=@Name2 )AND ParentCategoryId=@ParentCategoryId))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Category Name is Already Exists.'
				   END
			   ELSE
				   BEGIN
					   UPDATE [dbo].[Category]
					   SET [Name1] = @Name1
						  ,[Name2] = @Name2
						  ,[ParentCategoryId]=@ParentCategoryId
								   ,[CategoryLevel]=@CategoryLevel
								   ,[FileLocation]=@FileLocation
								   ,[FileName]=@FileName
								   ,[FileExtention]=@FileExtension
						  ,[IsActive] = @IsActive
						  ,[UpdatedByUserId] = @UpdatedByUserId
						  ,[UpdatedDate] = GETDATE()
					    WHERE Id= @Id
						
						 
						SET @StatusCode = @Id
						SET @StatusMessage='Category Updated Successfully'
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
		ROLLBACK TRANSACTION AddUpdateCategory;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AddUpdateCategory: %d: %s', 16, 1, @error, @message);
 END CATCH
END