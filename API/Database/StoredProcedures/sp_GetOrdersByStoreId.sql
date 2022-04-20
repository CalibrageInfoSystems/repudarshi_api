-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetOrdersByStoreId]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetOrdersByStoreId];
GO
CREATE PROCEDURE [dbo].[GetOrdersByStoreId]
@UserId INT,   
@StoreId INT = NULL,  
@StatusTypeId INT=NULL,
@FromDate DATETIME = NULL,  
@ToDate DATETIME = NULL  
AS  
BEGIN   
-- =============================================  
-- Author     : Parvathi  
-- Create date: 18/03/2020  
-- Description: Get all Categories  
-- Sample Call: EXEC dbo.[GetOrdersByStoreId] 3,NULL,NULL, NULL 
-- =============================================  
  
 SET NOCOUNT ON;  
  SELECT O.[Id]  
     ,[Code]  
     ,O.[UserId]  
     ,CustomerName = U.FirstName + ' ' +(CASE WHEN ISNULL(U.MiddleName,'')= '' THEN '' ELSE U.MiddleName+' ' END) + U.LastName  
     ,CustomerContactNumber =U.ContactNumber     
     ,[TotalPrice]  
     ,O.[StoreId]  
     ,S.Name1 AS 'StoreName1'  
     ,S.Name2 AS 'StoreName2'  
     ,S.CityName AS 'StoreCityName'  
     ,S.Landmark AS 'StoreLandmark'  
     ,O.[Address]  
     ,O.[Landmark]  
     ,O.[CityName]  
     ,O.[PostalCode]  
     ,ShippingAddress = O.Address + ', ' + O.Landmark + ', ' + O.CityName +', '+O.PostalCode 
     ,[StatusTypeId]  
     ,Status = TCD.[Desc]  
     ,[Comments]  
     ,O.[CreatedByUserId]  
     ,O.[CreatedDate]  
     ,O.[UpdatedByUserId]  
     ,O.[UpdatedDate]  
     ,O.[DeliveryAgentId]       
     ,DeliveryAgentName = UI.FirstName + ' ' +(CASE WHEN ISNULL(UI.MiddleName,'')= '' THEN '' ELSE UI.MiddleName+' ' END) + UI.LastName  
     ,DeliveryAgentContactNumber =UI.ContactNumber 
	 ,Convert(DATE,O.DeliveryDate) AS DeliveryDate
	 ,O.TimeSlot
  FROM [dbo].[Order] O  
  INNER JOIN [dbo].[TypeCdDmt] TCD ON O.StatusTypeId = TCD.TypeCdId  
  INNER JOIN [dbo].[Store] S ON S.Id = O.StoreId 
  INNER JOIN [dbo].[UserStoreXref] USX ON S.Id = USX.StoreId 
  LEFT JOIN dbo.UserInfo UI ON UI.Id = O.DeliveryAgentId  
  LEFT JOIN dbo.UserInfo U ON U.Id = O.UserId  
 WHERE USX.UserId = @UserId AND (@StoreId IS NULL OR O.StoreId=@StoreId)   AND (@StatusTypeId IS NULL OR O.StatusTypeId=@StatusTypeId)  
 AND ((@FromDate IS NULL AND @ToDate IS NULL) OR (CONVERT(date,O.CreatedDate) BETWEEN CONVERT(date,@FromDate) AND CONVERT(date,@ToDate)))  
 ORDER BY O.CreatedDate DESC
 END  
  
  