
CREATE FUNCTION [dbo].[HierarchicalUser] 
(    
      @Input INT
)
RETURNS  VARCHAR(1000)

AS
BEGIN
Declare @OutPUT VARCHAR(MAX);

WITH tblChild AS
(
    SELECT UserInfo.Id FROM UserInfo WHERE UserInfo.Id = @Input
    UNION ALL
    SELECT UserInfo.Id  FROM UserInfo  JOIN tblChild  ON UserInfo.ManagerId = tblChild.Id
)


select @OutPUT=  STUFF((SELECT ',' + CAST(Id AS VARCHAR(1000)) FROM tblChild  FOR XML PATH('')),1,1,'') ;
   
return 	 @OutPUT  

      
END
