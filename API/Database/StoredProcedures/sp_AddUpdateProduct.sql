-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateProduct]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateProduct];
GO
CREATE PROCEDURE [dbo].[AddUpdateProduct]
    @Id int = NULL,
	@Name1 nvarchar(100),
	@Name2 nvarchar(100),
	@Code varchar(50),
	@Description1 nvarchar(2000) = NULL,
	@Description2 nvarchar(2000) = NULL,
	@Price float,
	@DiscountedPrice float = NULL,
	@IsActive bit,
	@CreatedByUserId int,
	@CreatedDate datetime,
	@UpdatedByUserId int,
	@UpdatedDate datetime,
	@CategoryId INT,
	@FileName VARCHAR(250),
	@FileLocation VARCHAR(1000),
	@FileExtension VARCHAR(50),
	@StatusCode INT OUTPUT,
    @StatusMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
-- =============================================
-- Author:		<Bhavani>
-- Create date: <19-03-2020>
-- Description:	<Add or Update Store>
-- Sample Call: EXEC [AddUpdateProduct] 'SPAR','SPAR',NULL,NULL,NULL,'HYD','JNTU','HYD',NULL,1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
  BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AddUpdateProduct;
	  IF ISNULL(@Id,0) = 0
		          BEGIN	 
			   IF EXISTS (SELECT 1 FROM dbo.[Product] WHERE (Code=@Code OR Name1= @Name1 OR Name2=@Name2))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Product Name or Code is Already Exists.'
				   END
			   ELSE
				   BEGIN
				        INSERT INTO [dbo].[Product]
								   ([Name1]
								   ,[Name2]
								   ,[Code]
								   ,[Description1]
								   ,[Description2]
								   ,[Price]
								   ,[DiscountedPrice]
								   ,[IsActive]
								   ,[CreatedByUserId]
								   ,[CreatedDate]
								   ,[UpdatedByUserId]
								   ,[UpdatedDate])
						 VALUES (@Name1
						         ,@Name2
								 ,@Code
								 ,@Description1
								 ,@Description2
								 ,@Price
								 ,@DiscountedPrice
								 ,@IsActive
								 ,@CreatedByUserId
								 ,GETDATE()
								 ,@UpdatedByUserId
								 ,GETDATE()
								)
						SET @StatusCode = SCOPE_IDENTITY()	
						
						INSERT INTO [dbo].[ProductCategoryXref]
								   ([ProductId]
								   ,[CategoryId])
						VALUES
								   (@StatusCode
								   ,@CategoryId)	
								   
						INSERT INTO [dbo].[ProductRepository]
							   ([ProductId]
							   ,[FileName]
							   ,[FileLocation]
							   ,[FileExtension]
							   ,[IsDefault]
							   ,[IsActive]
							   ,[CreatedByUserId]
							   ,[CreatedDate]
							   ,[UpdatedByUserId]
							   ,[UpdatedDate])
						 VALUES
							   (@StatusCode
							   ,@FileName
							   ,@FileLocation
							   ,@FileExtension
							   ,1
							   ,@IsActive
							   ,@CreatedByUserId
							   ,GETDATE()
							   ,@UpdatedByUserId
							   ,GETDATE()
							   )								
						
						SET @StatusMessage='Product Added Successfully'
			      END
		    END
	  ELSE
		        BEGIN		
				     IF EXISTS (SELECT 1 FROM dbo.[Product] WHERE Id != @Id AND (Code=@Code OR Name1= @Name1 OR Name2=@Name2))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Product Name or Code is Already Exists.'
				   END
			   ELSE
				   BEGIN
					   UPDATE [dbo].[Product]
					   SET [Name1] = @Name1
						  ,[Name2] = @Name2
						  ,[Code] = @Code
						  ,[Description1] = @Description1
						  ,[Description2] = @Description2
						  ,[Price] = @Price
						  ,[DiscountedPrice] = @DiscountedPrice
						  ,[IsActive] = @IsActive
						  ,[UpdatedByUserId] = @UpdatedByUserId
						  ,[UpdatedDate] = GETDATE()
					    WHERE Id= @Id
						
						DELETE FROM [dbo].[ProductCategoryXref] WHERE ProductId= @Id
						INSERT INTO [dbo].[ProductCategoryXref]
								   ([ProductId]
								   ,[CategoryId])
						VALUES
								   (@Id
								   ,@CategoryId)	
					   UPDATE [dbo].[ProductRepository]
					   SET 
						  [FileName] = @FileName
						  ,[FileLocation] = @FileLocation
						  ,[FileExtension] = @FileExtension
						  ,[UpdatedByUserId] = @UpdatedByUserId
						  ,[UpdatedDate] = GETDATE()
					  WHERE ProductId = @Id
						SET @StatusCode = @Id
						SET @StatusMessage='Product Updated Successfully'
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
		ROLLBACK TRANSACTION AddUpdateProduct;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AddUpdateProduct: %d: %s', 16, 1, @error, @message);
 END CATCH
END
