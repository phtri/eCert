/*CERTIFICATES - INSERT*/

/*CERTIFICATES - DELETE*/

/*ORGANIZATIONS - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Organizations]
@OrganizationName		VARCHAR(50),
@LogoImage				VARCHAR(20),
AS
BEGIN
DECLARE @ActionStatus integer = 0;
	SET NOCOUNT ON;
	BEGIN TRY
		INSERT INTO Organizations(
			OrganizationName,
			LogoImage
			)
		VALUES(@OrganizationName, @LogoImage)
		
		IF @@ROWCOUNT > 0
			SET @ActionStatus = 1;
	END TRY		
	BEGIN CATCH
		SET @ActionStatus = @@ERROR
		PRINT 'Error: %1!, %2!.[Failed to insert data.]'
	END CATCH
	SET NOCOUNT OFF 
	RETURN @ActionStatus;
END