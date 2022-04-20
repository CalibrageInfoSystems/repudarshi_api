CREATE PROCEDURE [dbo].[GetCategoriesByParentCategoryId] 
@CategoryId INT = NULL
AS
BEGIN	
-- =============================================
-- Author     :	Parvathi
-- Create date: 18/03/2020
-- Description: Get Categories By Parent Category Id
-- Sample Call: EXEC dbo.[GetCategoriesByParentCategoryId]
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
	WHERE (@CategoryId IS NULL OR C.ParentCategoryId=@CategoryId)
	END
