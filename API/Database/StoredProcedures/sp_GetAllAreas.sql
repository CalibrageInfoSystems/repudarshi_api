CREATE PROCEDURE [dbo].[GetAllAreas] 
    @CityName VARCHAR(100) 
AS
BEGIN	
-- =============================================
-- Author:		<Prabhakar>
-- Create date: <08.01.2018>
-- Description:	<Get all Roles>
-- Sample Call: EXEC dbo.[GetAllAreas] 'Riyadh'
-- =============================================
	SET NOCOUNT ON;
	SELECT DISTINCT  CI.Code AS 'CityCode',
	                 CI.Id AS 'CityId',
					 CI.CountryId AS 'CountryId',
	                 CI.Name1 AS 'CityName1',
	                 CI.Name2 AS 'CityName2',
					 L.Id AS 'AreaId',
	                 L.Name1 AS 'AreaName1',
	                 L.Name2 AS 'AreaName2'
    FROM [dbo].[Location] L
    INNER JOIN [dbo].[City] CI ON CI.Id = L.CityId
	WHERE (CI.Name1 LIKE '%' + @CityName + '%' ) OR (CI.Name2 LIKE '%' + @CityName + '%' )
END


