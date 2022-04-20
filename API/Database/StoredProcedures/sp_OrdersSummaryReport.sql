-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[OrdersSummaryReport]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[OrdersSummaryReport];
GO
CREATE PROCEDURE [dbo].[OrdersSummaryReport]
@AgentId INT  = NULL,  
@CustomerId INT  = NULL,  
@StoreId INT = NULL,  
@StatusTypeId INT=NULL,
@FromDate DATETIME = NULL,  
@ToDate DATETIME = NULL  
AS  
BEGIN   
-- =============================================  
-- Author     : Harshitha  
-- Create date: 10/04/2020  
-- Description: OrdersSummaryReport 
-- Sample Call: EXEC dbo.[OrdersSummaryReport] NULL,NULL, 3,NULL,NULL, NULL 
-- =============================================  
  
 SET NOCOUNT ON;

select 
O.Id,
O.Code,
O.UserId,
O.TotalPrice,
O.StoreId,
O.StatusTypeId,
O.DeliveryAgentId ,
O.CreatedDate,
[Address]=O.[Address] + ', '+O.[LandMark]+', '+O.CityName+', '+O.PostalCode ,
AgentName=A.FirstName + ' ' + (CASE WHEN ISNULL(A.MiddleName, '') = '' THEN '' ELSE A.MiddleName + ' ' END) + A.LastName ,
CustomerName=C.FirstName + ' ' + (CASE WHEN ISNULL(C.MiddleName, '') = '' THEN '' ELSE C.MiddleName + ' ' END) + C.LastName ,
OrderStatus=t.[Desc],
StoreName1=S.Name1,
StoreName2=S.Name2 
 from [Order] O
 INNER JOIN  [dbo].[Store] (NOLOCK) S on S.Id=O.StoreId
 INNER JOIN  [dbo].[TypeCdDmt] (NOLOCK) T on T.TypeCdId=O.StatusTypeId
 INNER JOIN [dbo].[UserInfo] (NOLOCK) C on C.Id=O.UserId
 LEFT JOIN [dbo].[UserInfo] (NOLOCK) A on A.Id=O.DeliveryAgentId
 WHERE (@CustomerId IS NULL OR O.UserId=@CustomerId)   AND(@AgentId IS NULL OR O.DeliveryAgentId=@AgentId)   AND (@StoreId IS NULL OR O.StoreId=@StoreId)   AND (@StatusTypeId IS NULL OR O.StatusTypeId=@StatusTypeId)  
 AND ((@FromDate IS NULL AND @ToDate IS NULL) OR (CONVERT(date,O.CreatedDate) BETWEEN CONVERT(date,@FromDate) AND CONVERT(date,@ToDate)))  
 END