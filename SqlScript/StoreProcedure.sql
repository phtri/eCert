/*CERTIFICATES - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificate]
@CertificateName		NVARCHAR(50),
@VerifyCode				VARCHAR(100),
@Issuer					VARCHAR(20),
@Description			NVARCHAR(200),
@Hashing				VARCHAR(200),
@SubjectCode			VARCHAR(50),
@ViewCount				INT,
@DateOfIssue			DATETIME,
@DateOfExpiry			DATETIME,
@UserId					INT,
@OrganizationId			INT
AS
BEGIN
		INSERT INTO [dbo].[Certificate]
           ([CertificateName]
           ,[VerifyCode]
           ,[Issuer]
           ,[Description]
           ,[Hashing]
		   ,[SubjectCode]
		   ,[ViewCount]
		   ,[DateOfIssue]
		   ,[DateOfExpiry]
           ,[UserId]
           ,[OrganizationId]
           )
		VALUES
           (@CertificateName
           ,@VerifyCode
           ,@Issuer
           ,@Description
           ,@Hashing
		   ,@SubjectCode
		   ,@ViewCount
		   ,@DateOfIssue
		   ,@DateOfExpiry	
           ,@UserId
           ,@OrganizationId
           )
		   SELECT SCOPE_IDENTITY() 
END
DROP PROC sp_Insert_Certificate

/*CERTIFICATES - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_Certificate]
@CertificateId	INT
AS
BEGIN	
	DELETE FROM [dbo].[Certificate]
	WHERE CertificateId = @CertificateId
END

/*CertificateContents - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_CertificateContent]
@Content				VARCHAR(200),
@CertificateFormat		VARCHAR(20),
@CertificateId			INT
AS
BEGIN
		INSERT INTO [dbo].[CertificateContent]
			   ([Content]
			   ,[CertificateFormat]
			   ,[CertificateId]
			   )
		VALUES
			   (@Content
			   ,@CertificateFormat
			   ,@CertificateId
			   )
			   SELECT SCOPE_IDENTITY() 
END
DROP PROC sp_Insert_CertificateContent

/*ORGANIZATIONS - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Organization]
@OrganizationName		VARCHAR(50),
@LogoImage				VARCHAR(20)
AS
BEGIN
		INSERT INTO Organization(
			OrganizationName,
			LogoImage
			)
		VALUES(@OrganizationName, @LogoImage)
END

