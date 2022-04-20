CREATE PROCEDURE [dbo].[GetProductsByUserCartId]  
(
@UserCartId INT 
)
AS BEGIN
SELECT    UC.Id,Uc.UserCartId,UC.Quantity,    
                P.Id AS ProductId,
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
	FROM UserCartProductXref UC
	
	 INNER JOIN dbo.Product (NOLOCK) P ON P.Id=UC.ProductId 
	INNER JOIN dbo.ProductRepository PR ON PR.ProductId = P.Id 
	WHERE UC.UserCartId=@UserCartId 
	END