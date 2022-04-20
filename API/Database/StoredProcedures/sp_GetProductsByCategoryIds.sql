CREATE PROCEDURE [dbo].[GetProductsByCategoryIds]  -- '9,10'
(
	@CategoryId NVARCHAR(1000) = NULL,
	@PageNo INT = 1,
	@PageSize INT = 10,
	@SortColumn NVARCHAR(20) = 'Discount',
	@SortOrder NVARCHAR(20) = 'ASC'
)
 AS BEGIN
 SET NOCOUNT ON;
 ; WITH CTE_Results AS 
(
    SELECT         P.Id AS ProductId,
				   P.Code,
				   P.Name1,
				   P.Name2,
				   P.Description1,
				   P.Description2,
				   P.Price,
				   P.DiscountedPrice,
				   P.IsActive,
				   P.CreatedByUserId,
				   P.CreatedDate,
				   P.UpdatedByUserId,
				   P.UpdatedDate,
				   PR.FileName,
				   PR.FileExtension,
				   PR.FileLocation,
				    PR.FileLocation+'\\'+PR.FileName + PR.FileExtension  AS 'Filepath', 
				   (CASE WHEN P.Price = P.DiscountedPrice THEN 0 
				         ELSE CAST(ROUND((CAST((P.Price-P.DiscountedPrice) AS FLOAT)/P.Price)*100,2) AS INT) END) 
				    AS DiscountPercentage,
					 (CASE WHEN P.Price = P.DiscountedPrice THEN P.Price
				        ELSE CAST((P.Price-P.DiscountedPrice) AS FLOAT) END) AS ActualPrice
	FROM dbo.Product (NOLOCK) P
	INNER JOIN dbo.ProductCategoryXref PC ON PC.ProductId = P.Id
	INNER JOIN dbo.ProductRepository PR ON PR.ProductId = P.Id AND PR.IsDefault =1 AND PR.IsActive = 1
	WHERE P.IsActive = 1 
	AND PC.CategoryId IN (SELECT Item FROM dbo.SplitString(@CategoryId,','))
	 	  GROUP BY
		   P.Id ,
				   P.Code,
				   P.Name1,
				   P.Name2,
				   P.Description1,
				   P.Description2,
				   P.Price,
				   P.DiscountedPrice,
				   P.IsActive,
				   P.CreatedByUserId,
				   P.CreatedDate,
				   P.UpdatedByUserId,
				   P.UpdatedDate,
				   PR.FileName, 
				   PR.FileExtension,
				   PR.FileLocation 
		    ORDER BY
      CASE WHEN (@SortColumn = 'Discount' AND @SortOrder='ASC')
                    THEN (CASE WHEN P.Price = P.DiscountedPrice THEN 0 
				         ELSE CAST(ROUND((CAST((P.Price-P.DiscountedPrice) AS FLOAT)/P.Price)*100,2) AS INT) END) 
				 
        END ASC,
        CASE WHEN (@SortColumn = 'Discount' AND @SortOrder='DESC')
                   THEN (CASE WHEN P.Price = P.DiscountedPrice THEN 0 
				         ELSE CAST(ROUND((CAST((P.Price-P.DiscountedPrice) AS FLOAT)/P.Price)*100,2) AS INT) END) 
				    
		END DESC,
	   CASE WHEN (@SortColumn = 'Newest' AND @SortOrder='ASC')
                    THEN P.CreatedDate
        END ASC,
        CASE WHEN (@SortColumn = 'Newest' AND @SortOrder='DESC')
                   THEN P.CreatedDate
		END DESC,
		 CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')
                    THEN (CASE WHEN P.Price = P.DiscountedPrice THEN P.Price
				        ELSE CAST((P.Price-P.DiscountedPrice) AS FLOAT) END)
        END ASC,
        CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')
                   THEN (CASE WHEN P.Price = P.DiscountedPrice THEN P.Price
				        ELSE CAST((P.Price-P.DiscountedPrice) AS FLOAT) END)
		END DESC
      OFFSET @PageSize * (@PageNo - 1) ROWS
      FETCH NEXT @PageSize ROWS ONLY
	),
CTE_TotalRows AS 
(
   SELECT  COUNT(ProductId) AS MaxRows
   FROM CTE_Results
-- SELECT  COUNT(PC.ProductId) AS MaxRows
--FROM dbo.Product (NOLOCK) P
--	INNER JOIN dbo.ProductCategoryXref PC ON PC.ProductId = P.Id 
--	WHERE P.IsActive = 1 
--	AND PC.CategoryId IN (SELECT Item FROM dbo.SplitString(@CategoryId,','))
)
   Select MaxRows, P.ProductId,
				   P.Code,
				   P.Name1,
				   P.Name2,
				   P.Description1,
				   P.Description2,
				   ROUND(P.Price,2) As Price,
				   ROUND(P.DiscountedPrice,2) AS DiscountedPrice,
				   P.IsActive,
				   P.CreatedByUserId,
				   P.CreatedDate,
				   P.UpdatedByUserId,
				   P.UpdatedDate,
				   P.FileName,
				   P.FileExtension,
				   P.FileLocation ,
				   P.Filepath 
   FROM CTE_Results AS P, CTE_TotalRows    
   END
