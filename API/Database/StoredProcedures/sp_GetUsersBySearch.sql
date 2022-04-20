CREATE  PROCEDURE GetUsersBySearch
(
@Search VARCHAR(500),
@RoleId INT
)
AS
-- =============================================  
-- Author     : Harshitha  
-- Create date: 10/04/2020  
-- Description: GetUsersBySearch 
-- Sample Call: EXEC dbo.[GetUsersBySearch]  ' ',4
-- ============================================= 
BEGIN
  
SET NOCOUNT ON; 
select 
Id,
UserName,
U.ContactNumber,
U.Email,
FirstName,
MiddleName,
LastName,
FullName=U.FirstName +' '+  (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName 


from UserInfo U

WHERE U.RoleId=@RoleId AND

(U.FirstName + ' ' + (CASE WHEN ISNULL(U.MiddleName, '') = '' THEN '' ELSE U.MiddleName + ' ' END) + U.LastName) like '%'+@Search+'%'

END