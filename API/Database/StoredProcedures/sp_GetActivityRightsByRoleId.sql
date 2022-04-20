-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetActivityRightsByRoleId]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetActivityRightsByRoleId];
GO
CREATE PROCEDURE [dbo].[GetActivityRightsByRoleId] 
   	@RoleId int
   AS
   BEGIN	
   -- GET AcivityRights By RoleId
   -- =============================================
   -- Author:		<Prabhakar>
   -- Create date: <13.03.2016>
   -- Description:	<Get Role by Id>
   -- Sample Call: EXEC dbo.GetActivityRightsByRoleId @RoleId = 1
   -- =============================================
   	SET NOCOUNT ON;
   		SELECT 
   				ar.Id,
   				ar.Name,
   				ar.[Desc],
   				ar.IsActive,
   				ar.CreatedByUserId,
   				ar.CreatedDate,
   				ar.UpdatedByUserId,
   				ar.UpdatedDate	
   			FROM dbo.ActivityRight (NOLOCK) ar			
   			INNER JOIN dbo.RoleActivityRightXref rax ON rax.ActivityRightId = ar.Id	
   			WHERE rax.RoleId=@RoleId
   		
   END
