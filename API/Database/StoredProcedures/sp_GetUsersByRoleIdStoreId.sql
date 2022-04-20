CREATE  PROCEDURE [dbo].[GetUsersByRoleIdStoreId]
 @RoleId nvarchar(50) =NULL,
  @StoreId INT =NULL
   AS
   BEGIN	
   -- =============================================
   -- Author     : Rajeswari
   -- Create date: 03/10/2019
   -- Description: Get Users By RoleId
   -- Sample Call: EXEC dbo.[GetUsersByRoleIdStoreId]  '1,2'
   -- =============================================	
   SET NOCOUNT ON;
   	SELECT 
   		U.Id,
		U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName As FullName,
   		U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName  +' '+'('+R.Code+')' AS UserName,
   		U.RoleId,
   		R.NAME AS 'RoleName',
		R.Code AS 'Code',
		StoreName1=s.Name1,
		StoreName2=S.Name2,
		AccessToken=''
     	FROM [dbo].[UserInfo] (NOLOCK) U
   		INNER JOIN [dbo].[Role] R (NOLOCK) ON U.RoleId = R.Id 		
		INNER JOIN [dbo].[UserStoreXref] US (NOLOCK) ON U.Id = US.UserId
		INNER JOIN [dbo].[Store] S (NOLOCK) ON S.Id = US.UserId
	 WHERE ( @StoreId IS NULL OR US.StoreId = @StoreId) AND ( @RoleId IS NULL OR U.RoleId IN (SELECT Item FROM dbo.SplitString(@RoleId,',')))
		
   END	
