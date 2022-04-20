-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[GetActivityRights]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[GetActivityRights];
GO
CREATE PROCEDURE [dbo].[GetActivityRights] 
 	@Id INT = NULL
 AS
 BEGIN	
 -- GET ActivityRights
 -- =============================================
 -- Author:		<Prabhakar>
 -- Create date: <14.03.2016>
 -- Description:	<Get ActivityRights by Id>
 -- Sample Call: EXEC dbo.GetActivityRights @Id = 0
 -- =============================================
 	SET NOCOUNT ON;
 		SELECT a.Id,
 			   a.Name,
 			   a.[Desc],
 			   a.IsActive,
 			   a.CreatedByUserId,
 			   a.CreatedDate,
 			   a.UpdatedByUserId,
 			   a.UpdatedDate
 	FROM dbo.ActivityRight (NOLOCK) a
 	WHERE (@Id IS NULL OR a.Id=@Id) AND a.IsActive=1
 	END

