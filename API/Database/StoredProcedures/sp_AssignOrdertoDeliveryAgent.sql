-- IF PROCEDURE ALREADY EXISTS
IF OBJECT_ID ('dbo.[AssignOrdertoDeliveryAgent]', 'P') IS NOT NULL
    DROP PROCEDURE dbo.[AssignOrdertoDeliveryAgent];
GO
CREATE PROCEDURE [dbo].[AssignOrdertoDeliveryAgent] 
@OrderId INT,
@DeliveryAgentId INT,
@UpdatedByUserId INT,
@UpdatedDate DATETIME,
@StatusCode INT OUTPUT,
@StatusMessage NVARCHAR(MAX) OUTPUT
AS
BEGIN
-- =============================================
-- Author:		<Bhavani>
-- Create date: <19-03-2020>
-- Description:	<Add or Update City>
-- Sample Call: EXEC AssignOrdertoDeliveryAgent null,'Guntur','Guntur','Gun',1,1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
 
 BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION AssignOrdertoDeliveryAgent ;
			--If OrderId
		IF EXISTS (SELECT 1 FROM dbo.[Order] WHERE (Id=@OrderId AND  ISNULL(DeliveryAgentId,'')=''))
		  BEGIN
			
			--INSERT INTO OrderStatusHistory
			--SELECT Id
			--      ,StatusTypeId
			--	  ,UpdatedByUserId
			--	  ,UpdatedDate
			--	  ,@UpdatedByUserId
			--	  ,@UpdatedDate
			--	  ,Comments
		 --   FROM dbo.[Order]
			--WHERE Id = @OrderId
			
			UPDATE dbo.[Order]
			SET DeliveryAgentId = @DeliveryAgentId,
			    --StatusTypeId = 6,
				UpdatedByUserId = @UpdatedByUserId,
				UpdatedDate = @UpdatedDate
		    WHERE Id = @OrderId
	  
			SET @StatusCode = SCOPE_IDENTITY();
			SET @StatusMessage='Order Has Been Assigned to the Delivery Agent'
			END
			ELSE
			--ELSE
			BEGIN
				SET @StatusCode = -1;
			SET @StatusMessage='Order Has Been Already Assigned to other  Delivery Agent'
			END
				   
		IF @trancount = 0
            COMMIT;
 END TRY
 BEGIN CATCH
	DECLARE @error int, @message NVARCHAR(4000), @xstate int;
	select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
	if @xstate = -1
		ROLLBACK;
	if @xstate = 1 and @trancount = 0
		ROLLBACK
	if @xstate = 1 and @trancount > 0
		ROLLBACK TRANSACTION AssignOrdertoDeliveryAgent ;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('AssignOrdertoDeliveryAgent : %d: %s', 16, 1, @error, @message);
 END CATCH
END
