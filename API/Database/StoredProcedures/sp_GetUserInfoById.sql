-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetUserInfoById]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetUserInfoById;
GO
CREATE PROCEDURE [dbo].[GetUserInfoById]	
   	@Id INT=NULL
   AS
   BEGIN	
   -- =============================================
   -- Author     : Harshitha
   -- Create date: 17/09/2019
   -- Description:	Get UserInfo by Id 
   -- Sample Call: EXEC dbo.[GetUserInfoById] NULL
   -- =============================================	
   SET NOCOUNT ON;
   	SELECT 
   		U.Id,
   		U.UserId,
   		U.FirstName,
   		U.MiddleName,
   		U.LastName,
   		U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName AS 'FullName',
   		U.UserName,
   		U.Password,
   		U.RoleId,
   		R.NAME AS 'RoleName',  
		U.ManagerId,
		STUFF((SELECT DISTINCT ',' + CAST(ActivityRightId AS VARCHAR(10)) FROM [dbo].[RoleActivityRightXref]
			          WHERE RoleId = R.Id FOR XML PATH('')),1,1,'') AS 'ActivityRights',
		MR.FirstName + ' ' + (CASE WHEN ISNULL(MR.MiddleName, '') = '' THEN '' ELSE MR.MiddleName + ' ' END) + MR.LastName AS 'ManagerName',
   		U.ContactNumber, 
		U.Address, 
   		U.Email,  
   		U.IsActive,
   		U.CreatedByUserId,
   		U.CreatedDate,
   		U.UpdatedByUserId,
   		U.UpdatedDate, 
		U.Landmark,
		U.Country AS 'CountryId',
		U.City AS 'CityId',
		U.Location AS 'LocationId',
		C.Name1 AS 'CountryName1',
		C.Name2 AS 'CountryName2',
		CC.Name1 AS 'CityName1',
		CC.Name2 AS 'CityName2',
		L.Name1 AS 'LocationName1',
		L.Name2 AS 'LocationName2',
		 STUFF((SELECT ',' + CAST(StoreId AS VARCHAR(10)) FROM [dbo].[UserStoreXref] 
		 	          WHERE UserId = U.Id FOR XML PATH('')),1,1,'') AS StoreIds 
	 
     	FROM [dbo].[UserInfo] (NOLOCK) U
   		INNER JOIN [dbo].[Role] R (NOLOCK) ON U.RoleId = R.Id  
		LEFT JOIN dbo.UserInfo MR (NOLOCK) ON U.ManagerId=MR.Id
		LEFT JOIN dbo.Country C (NOLOCK) ON U.Country=C.Id
		LEFT JOIN dbo.City CC (NOLOCK) ON U.City=CC.Id
		LEFT JOIN dbo.Location L (NOLOCK) ON U.Location=L.Id
   	WHERE (@Id IS NULL OR U.Id=@Id) 
   END	

