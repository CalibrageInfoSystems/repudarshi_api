-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetAllStores]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetAllStores];
GO
CREATE PROCEDURE [dbo].[GetAllStores] 
	@Id INT = NULL,
	@IsActive BIT=NULL
AS
BEGIN	
-- =============================================
-- Author:		<Bhavani>
-- Create date: <19-03-2020>
-- Description:	<Get all Stores>
-- Sample Call: EXEC dbo.GetAllStores null,null
-- =============================================
	SET NOCOUNT ON;
		SELECT s.Id,
		s.Name1,
		s.Name2,
		s.FileName,
		s.FileLocation,
		s.FileExtension,
		s.FileLocation+'\\'+s.FileName + s.FileExtension  AS 'Filepath',
		s.Address,
		s.Landmark,
		s.CityName,
		s.PostalCode,
		s.Address+','+s.Landmark +','+s.CityName+(CASE WHEN ISNULL(s.PostalCode, '') = '' THEN '' ELSE s.PostalCode  END) as FullAddress,
		s.IsActive,
		s.CreatedByUserId,
		s.CreatedDate,
		s.UpdatedByUserId,
		s.UpdatedDate
	FROM Store (NOLOCK) s
	
	WHERE (@Id IS NULL OR s.Id=@Id) AND (@IsActive IS NULL OR s.IsActive=@IsActive)
	ORDER BY s.UpdatedDate DESC
	END
