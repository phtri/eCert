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
/*CERTIFICATE_USER - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificate_User]
@RollNumber				varchar(10),
@CertificateId			INT
AS
BEGIN
		INSERT INTO Certificate_User(
			RollNumber,
			CertificateId
			)
		VALUES(@RollNumber, @CertificateId)
END

DROP PROC sp_Insert_Certificate_User
/*CERTIFICATE_USER - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_Certificate_User]
@CertificateId			INT
AS
BEGIN
		DELETE FROM [dbo].[Certificate_User]
		WHERE CertificateId = @CertificateId
END
/*REPORT - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Report]
@ReportContent	NVARCHAR(100),
@Status			NVARCHAR(20),
@UserId			INT,
@CertificateId	INT,
@Title			NVARCHAR(100)
AS
BEGIN
		INSERT INTO [dbo].[Report]
			   (
			   [ReportContent],
			   [Status],
			   [UserId],
			   [CertificateId],
			   [Title]
			   )
		VALUES
			   (@ReportContent,
			   @Status,
			   @UserId,
			   @CertificateId,
			   @Title
			   )
			   SELECT SCOPE_IDENTITY() 
END

/*CAMPUS - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Campus]
@CampusName				NVARCHAR(100),
@EducationSystemId		INT
AS
BEGIN
			INSERT INTO [dbo].[Campus]
					(
					[CampusName],
					[EducationSystemId]
					)
			VALUES	
					(@CampusName,
					@EducationSystemId
					)
					SELECT SCOPE_IDENTITY()
END
/*CAMPUS - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_Campus]
@CampusId			INT
AS
BEGIN
		DELETE FROM [dbo].[Campus]
		WHERE CampusId = @CampusId
END
/*EDUCATION SYSTEM - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_EducationSystem]
@EducationSystemId			INT
AS
BEGIN
		DELETE FROM [dbo].[EducationSystem]
		WHERE EducationSystemId = @EducationSystemId
END
