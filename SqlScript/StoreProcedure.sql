/*CERTIFICATES - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificates]
@CertificateName		NVARCHAR(50),
@VerifyCode				VARCHAR(20),
@Issuer					VARCHAR(20),
@Description			NVARCHAR(200),
@Hashing				VARCHAR(200),
@ViewCount				INT,
@DateOfIssue			DATETIME,
@DateOfExpiry			DATETIME,
@UserId					INT,
@OrganizationId			INT,
@created_at				DATETIME,
@updated_at				DATETIME
AS
BEGIN
		INSERT INTO [dbo].[Certificates]
           ([CertificateName]
           ,[VerifyCode]
           ,[Issuer]
           ,[Description]
           ,[Hashing]
		   ,[ViewCount]
		   ,[DateOfIssue]
		   ,[DateOfExpiry]
           ,[UserId]
           ,[OrganizationId]
           ,[created_at]
           ,[updated_at])
		VALUES
           (@CertificateName
           ,@VerifyCode
           ,@Issuer
           ,@Description
           ,@Hashing
		   ,@ViewCount
		   ,@DateOfIssue
		   ,@DateOfExpiry	
           ,@UserId
           ,@OrganizationId
           ,@created_at
           ,@updated_at)
		   SELECT SCOPE_IDENTITY() 
END
DROP PROC sp_Insert_Certificates

/*CERTIFICATES - DELETE*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificates]
@CertificateId	INT
AS
BEGIN	
	DELETE FROM [dbo].[Certificates]
	WHERE CertificateId = @CertificateId
END

/*CertificateContents - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_CertificateContents]
@Content				VARCHAR(200),
@Format					VARCHAR(20),
@CertificateId			INT,
@created_at				DATETIME,
@updated_at				DATETIME
AS
BEGIN
		INSERT INTO [dbo].[CertificateContents]
			   ([Content]
			   ,[Format]
			   ,[CertificateId]
			   ,[created_at]
			   ,[updated_at])
		VALUES
			   (@Content
			   ,@Format
			   ,@CertificateId
			   ,@created_at
			   ,@updated_at)
			   SELECT SCOPE_IDENTITY() 
END
DROP PROC sp_Insert_CertificateContents

/*ORGANIZATIONS - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Organizations]
@OrganizationName		VARCHAR(50),
@LogoImage				VARCHAR(20)
AS
BEGIN
		INSERT INTO Organizations(
			OrganizationName,
			LogoImage
			)
		VALUES(@OrganizationName, @LogoImage)
END

