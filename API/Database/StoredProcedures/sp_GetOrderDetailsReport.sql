-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetOrderDetailsReport]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetOrderDetailsReport];
GO
CREATE PROCEDURE [dbo].[GetOrderDetailsReport]
@OrderCode VARCHAR(100)=NULL  
AS  
BEGIN   
-- =============================================  
-- Author     : Harshitha  
-- Create date: 10/04/2020  
-- Description: GetOrderDetailsReport 
-- Sample Call: EXEC dbo.[GetOrderDetailsReport]  'ORD26032008415356414'
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
OrderDate=O.CreatedDate,
O.[DeliveryDate],
O.TimeSlot,
[Address]=O.[Address] + ', '+O.[LandMark]+', '+O.CityName+', '+O.PostalCode ,
AgentName=A.FirstName + ' ' + (CASE WHEN ISNULL(A.MiddleName, '') = '' THEN '' ELSE A.MiddleName + ' ' END) + A.LastName ,
AgentContactNumber=A.ContactNumber,
CustomerName=C.FirstName + ' ' + (CASE WHEN ISNULL(C.MiddleName, '') = '' THEN '' ELSE C.MiddleName + ' ' END) + C.LastName ,
CustContactNumber=A.ContactNumber,
CustAddress=O.[Address] + ', '+O.[LandMark]+', '+O.CityName+', '+O.PostalCode ,
OrderStatus=t.[Desc],
StoreName1=S.Name1,
StoreName2=S.Name2 
 from [Order] O
 INNER JOIN  [dbo].[Store] (NOLOCK) S on S.Id=O.StoreId
 INNER JOIN  [dbo].[TypeCdDmt] (NOLOCK) T on T.TypeCdId=O.StatusTypeId
 INNER JOIN [dbo].[UserInfo] (NOLOCK) C on C.Id=O.UserId
 LEFT JOIN [dbo].[UserInfo] (NOLOCK) A on A.Id=O.DeliveryAgentId 
 WHERE (ISNULL(@OrderCode ,'')='' OR O.Code=@OrderCode)
 END
