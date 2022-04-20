CREATE PROCEDURE [dbo].[UpdateOrderStatus] 
@OrderId INT,
@StatusTypeId INT,
@Comments NVARCHAR(2500),
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
-- Sample Call: EXEC [UpdateOrderStatus] null,'Guntur','Guntur','Gun',1,1,1,'19-03-2020',1,'19-03-2020', @StatusCode OUTPUT, @StatusMessage OUTPUT
-- =============================================
 SET NOCOUNT ON;
 DECLARE @trancount INT;
 SET @trancount = @@trancount;
 
 BEGIN TRY
	IF @trancount = 0
            BEGIN TRANSACTION
        ELSE
            SAVE TRANSACTION UpdateOrderStatus ;
			INSERT INTO OrderStatusHistory
			SELECT Id
			      ,StatusTypeId
				  ,UpdatedByUserId
				  ,UpdatedDate
				  ,@UpdatedByUserId
				  ,@UpdatedDate
				  ,Comments
		    FROM dbo.[Order]
			WHERE Id = @OrderId
			
			UPDATE dbo.[Order]
			SET StatusTypeId = @StatusTypeId,
			    Comments = @Comments,
				UpdatedByUserId = @UpdatedByUserId,
				UpdatedDate = @UpdatedDate
		    WHERE Id = @OrderId
	  
			SET @StatusCode = SCOPE_IDENTITY();
			SET @StatusMessage='Order Status Has Been Updated Successfully'
				   
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
		ROLLBACK TRANSACTION UpdateOrderStatus ;
	SET @StatusMessage = ERROR_MESSAGE()
	SET @StatusCode=0
	RAISERROR ('UpdateOrderStatus : %d: %s', 16, 1, @error, @message);
 END CATCH
END



