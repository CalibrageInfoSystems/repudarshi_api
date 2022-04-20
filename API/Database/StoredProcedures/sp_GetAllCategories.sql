-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetAllCategories]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetAllCategories];
GO
CREATE PROCEDURE [dbo].[GetAllCategories] 
@Id INT = NULL,
@IsActive Bit = NULL
AS
BEGIN	
-- =============================================
-- Author     :	Parvathi
-- Create date: 18/03/2020
-- Description: Get all Categories
-- Sample Call: EXEC dbo.[GetAllCategories]
-- =============================================

	SET NOCOUNT ON;
		SELECT C.Id, 
			   C.Name1,
			   C.Name2,
			   C.CategoryLevel,
			   C.ParentCategoryId,
			   PC.Name1 AS 'ParentCategoryName1',
			   PC.Name2 AS 'ParentCategoryName2',
			   C.FileName,
			   C.FileExtention,
			   C.FileLocation,
			  C.FileLocation+'\\'+C.FileName + C.FileExtention  AS 'Filepath', 
			   C.IsActive,
			   C.CreatedByUserId,
			   C.CreatedDate,
			   C.UpdatedByUserId,
			   C.UpdatedDate
	FROM dbo.Category (NOLOCK) C
	LEFT JOIN dbo.Category PC ON PC.Id = C.ParentCategoryId
	WHERE (@Id IS NULL OR C.Id=@Id) AND (@IsActive IS NULL OR C.IsActive=@IsActive)
	END



