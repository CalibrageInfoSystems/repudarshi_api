-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetUserinfoByUserName]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetUserinfoByUserName];
GO
CREATE  PROCEDURE [dbo].[GetUserinfoByUserName] 
	@UserName NVARCHAR(50)   
AS
-- =============================================
-- Author:		<Prabhakar>
-- Create date: <16-06-2017>
-- Description:	Get User Info by Username>
-- Sample Call: EXEC GetUserinfoByUserName 'superadmin''
-- =============================================
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM [dbo].[UserInfo] u WHERE u.UserName=@UserName AND u.IsActive=1 )
		SELECT 
			u.Id,
			u.FirstName + ' ' + (CASE WHEN ISNULL(u.MiddleName, '') = '' THEN '' ELSE u.MiddleName + ' ' END) + u.LastName AS Name,	
			u.UserName,
			u.ContactNumber, 				
			u.RoleId, 
			u.Email,
			r.[NAME] + ' (' + r.Code + ')' AS 'RoleName'
		FROM [dbo].[UserInfo] u 
		INNER JOIN [dbo].[Role] r ON u.RoleId = r.Id AND r.IsActive = 1
		WHERE u.UserName=@UserName AND u.IsActive=1
	ELSE
	    SELECT NULL
END