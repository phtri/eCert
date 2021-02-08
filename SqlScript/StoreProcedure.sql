/*CERTIFICATES - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificates]
@CertificateName		NVARCHAR(50),
@VerifyCode				VARCHAR(20),
@Issuer					VARCHAR(20),
@Format					VARCHAR(10),
@Description			NVARCHAR(200),
@Hashing				VARCHAR(200),
@ViewCount				INT,
@UserId					INT,
@OrganizationId			INT,
@created_at				DATETIME,
@updated_at				DATETIME
AS
BEGIN
DECLARE @ActionStatus integer = 0;
	SET NOCOUNT ON;
	BEGIN TRY
		INSERT INTO [dbo].[Certificates]
           ([CertificateName]
           ,[VerifyCode]
           ,[Issuer]
           ,[Format]
           ,[Description]
           ,[Hashing]
		   ,[ViewCount]
           ,[UserId]
           ,[OrganizationId]
           ,[created_at]
           ,[updated_at])
		VALUES
           (@CertificateName
           ,@VerifyCode
           ,@Issuer
           ,@Format
           ,@Description
           ,@Hashing
		   ,@ViewCount
           ,@UserId
           ,@OrganizationId
           ,@created_at
           ,@updated_at)
		
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


/*CERTIFICATES - DELETE*/

/*ORGANIZATIONS - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Organizations]
@OrganizationName		VARCHAR(50),
@LogoImage				VARCHAR(20)
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