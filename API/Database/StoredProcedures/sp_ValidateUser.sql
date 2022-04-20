-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[ValidateUser]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[ValidateUser];
GO
CREATE PROCEDURE [dbo].[ValidateUser] 
 	@UserName NVARCHAR(50),
 	@Password NVARCHAR(50)    
 AS
 -- =============================================
 -- Author:		<Siva Prasad>
 -- Create date: <06-01-2017>
 -- Description:	<Validate User by Username & Password>
 -- Sample Call: EXEC ValidateUser 'superadmin', 'superadmin'
 -- =============================================
 BEGIN
 	SET NOCOUNT ON;
   	IF EXISTS(SELECT 1 FROM [dbo].[UserInfo] u WHERE u.UserName=@UserName AND u.Password=@Password AND u.IsActive=1)
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
 		WHERE u.UserName=@UserName AND u.Password=@Password AND u.IsActive=1
 	ELSE
 	    SELECT NULL
 END
