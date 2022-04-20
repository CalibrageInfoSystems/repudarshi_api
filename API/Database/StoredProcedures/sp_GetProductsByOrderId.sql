CREATE PROCEDURE [dbo].[GetProductsByOrderId] 
@OrderId INT
AS
BEGIN	
-- =============================================
-- Author     :	Parvathi
-- Create date: 18/03/2020
-- Description: Get all Categories
-- Sample Call: EXEC dbo.[GetProductsByOrderId]
-- =============================================

	SET NOCOUNT ON;
		SELECT [OrderId]
			  ,[ProductId]
			  ,P.Name1
			  ,P.Name2
			  ,P.Code
			  ,P.Description1
			  ,P.Description2
			  ,Price=ROUND(OPX.[Price],2)
			  ,Quantity=ROUND(OPX.[Quantity],2)
			  ,TotalPrice = ROUND(OPX.[Price] * OPX.[Quantity],2)
  FROM [dbo].[OrderProductXref] OPX
  INNER JOIN [dbo].[Product] P ON P.Id = OPX.ProductId
  WHERE  OPX.OrderId = @OrderId
END



