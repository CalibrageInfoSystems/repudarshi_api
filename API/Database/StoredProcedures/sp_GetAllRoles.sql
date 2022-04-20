-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetAllRoles]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetAllRoles];
GO
CREATE PROCEDURE [dbo].[GetAllRoles] 
	@Id INT = NULL,
	@IsActive BIT=NULL
AS
BEGIN	
-- GET Role
-- =============================================
-- Author:		<Prabhakar>
-- Create date: <08.01.2018>
-- Description:	<Get all Roles>
-- Sample Call: EXEC dbo.GetAllRoles null,0
-- =============================================
	SET NOCOUNT ON;
		SELECT r.Id, 
			   r.Code,
			   r.NAME,
			   r.[Desc],
			   r.ParentRoleId,
			   pr.NAME AS 'ParentRole',
			   r.IsActive,
			   r.CreatedByUserId,
			   r.CreatedDate,
			   r.UpdatedByUserId,
			   r.UpdatedDate
	FROM Role (NOLOCK) r
	LEFT JOIN dbo.Role PR ON PR.Id = R.ParentRoleId
	WHERE (@Id IS NULL OR r.Id=@Id) AND (@IsActive IS NULL OR r.IsActive=@IsActive)
	END
