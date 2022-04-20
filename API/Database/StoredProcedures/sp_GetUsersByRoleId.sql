-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetUsersByRoleId]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetUsersByRoleId];
GO
CREATE  PROCEDURE [dbo].[GetUsersByRoleId]
 @RoleId nvarchar(50) =NULL
   AS
   BEGIN	
   -- =============================================
   -- Author     : Rajeswari
   -- Create date: 03/10/2019
   -- Description: Get Users By RoleId
   -- Sample Call: EXEC dbo.[GetUsersByRoleId]  '1,2'
   -- =============================================	
   SET NOCOUNT ON;
   	SELECT 
   		U.Id,
		U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName As FullName,
   		U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName  +' '+'('+R.Code+')' AS UserName,
   		U.RoleId,
   		R.NAME AS 'RoleName',
		R.Code AS 'Code'
     	FROM [dbo].[UserInfo] (NOLOCK) U
   		INNER JOIN [dbo].[Role] R (NOLOCK) ON U.RoleId = R.Id  
	 WHERE ( @RoleId IS NULL OR U.RoleId IN (SELECT Item FROM dbo.SplitString(@RoleId,',')))
		
   END	
