-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AddUpdateRole]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AddUpdateRole];
GO
CREATE PROCEDURE [dbo].[AddUpdateRole]
@Id INT = NULL,
@Code NVARCHAR(50),
@Name NVARCHAR(255),
@Desc NVARCHAR(500) = NULL,
@ParentRoleId INT = NULL,
@ActivityRightIds NVARCHAR(1000),
@IsActive BIT = 1,
@CreatedByUserId INT,
@CreatedDate DATETIME,
@UpdatedByUserId INT,
@UpdatedDate DATETIME,
@StatusCode INT OUTPUT,
@StatusMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
 --SET XACT_ABORT ON
 -- ADD OR UPDATE Role
-- =============================================
-- Author:		<Harshitha>
-- Create date: <18.09.2019>
-- Description:	<Add or Update Role>
-- Sample Call: EXEC AddUpdateRole 'CEO', 'Cheif Executive Officer', 'Cheif Executive Officer',null, 1, '2017-01-09 16:48:56.780', 1, '2017-01-09 16:48:56.780',, @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
  
 BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AddUpdateRole;
	  IF ISNULL(@Id,0) = 0
		  BEGIN
			   IF EXISTS (SELECT 1 FROM dbo.[Role] WHERE (Code=@Code OR Name=@Name))
				   BEGIN
					   SET @StatusCode = -1
					   SET @StatusMessage='Role Name or Code Already Exists in the Role Table'
				   END
			   ELSE
				   BEGIN
						INSERT INTO dbo.[Role] (Code, Name, [Desc], ParentRoleId, IsActive, CreatedByUserId, CreatedDate, UpdatedByUserId, UpdatedDate) 
									  VALUES (@Code, @Name, @Desc, @ParentRoleId, @IsActive, @CreatedByUserId, @CreatedDate, @UpdatedByUserId, @UpdatedDate)
						SET @StatusCode = SCOPE_IDENTITY()
						INSERT INTO dbo.RoleActivityRightXref
						SELECT @StatusCode, Item FROM dbo.SplitString(@ActivityRightIds,',')
						
						SET @StatusMessage='Role Added Successfully'
				   END
		  END
	  ELSE
		  BEGIN
				IF EXISTS (SELECT 1 FROM dbo.[Role] WHERE Id != @Id AND (Code=@Code OR Name=@Name))
					BEGIN			   
						SET @StatusCode = -1
						SET @StatusMessage='Role Name or Code Already Exists in the Role Table'
					END
				ELSE
					BEGIN
						UPDATE dbo.[Role] SET Code=@Code, Name=@Name, [Desc]=@Desc, ParentRoleId = @ParentRoleId, IsActive=@IsActive, UpdatedByUserId=@UpdatedByUserId, UpdatedDate=@UpdatedDate WHERE Id=@Id
						DELETE FROM dbo.RoleActivityRightXref WHERE RoleId = @Id 
						INSERT INTO dbo.RoleActivityRightXref
						SELECT @Id, Item FROM dbo.SplitString(@ActivityRightIds,',')
						SET @StatusCode = @Id
						SET @StatusMessage='Role Updated Successfully'
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
		ROLLBACK TRANSACTION AddUpdateRole;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AddUpdateRole: %d: %s', 16, 1, @error, @message);
 END CATCH
END

