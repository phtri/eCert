/*CERTIFICATES - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificate]
@CertificateName			NVARCHAR(50),
@VerifyCode					VARCHAR(100),
@Url						VARCHAR(100),
@Issuer						VARCHAR(20),
@Description				NVARCHAR(200),
@Hashing					VARCHAR(200),
@ViewCount					INT,
@DateOfIssue				DATE,
@DateOfExpiry				DATE,
@SubjectCode				VARCHAR(50),
@RollNumber					VARCHAR(50),
@FullName					NVARCHAR(100),
@Nationality				NVARCHAR(100),
@PlaceOfBirth				NVARCHAR(100),
@Curriculum					VARCHAR(50),
@GraduationYear				DATE,
@GraduationGrade			NVARCHAR(100),
@GraduationDecisionNumber 	NVARCHAR(100),
@DiplomaNumber				NVARCHAR(100),
@OrganizationId				INT
AS
BEGIN
		INSERT INTO [dbo].[Certificate]
           ([CertificateName]
           ,[VerifyCode]
		   ,[Url]
           ,[Issuer]
           ,[Description]
           ,[Hashing]
		   ,[ViewCount]
		   ,[DateOfIssue]
		   ,[DateOfExpiry]
		   ,[SubjectCode]
		   ,[RollNumber]
		   ,[FullName]
		   ,[Nationality]
		   ,[PlaceOfBirth]
		   ,[Curriculum]
		   ,[GraduationYear]
		   ,[GraduationGrade]
		   ,[GraduationDecisionNumber]
		   ,[DiplomaNumber]
		   ,[OrganizationId]
           )
		VALUES
           (@CertificateName		
			,@VerifyCode				
			,@Url
			,@Issuer
			,@Description
			,@Hashing
			,@ViewCount
			,@DateOfIssue
			,@DateOfExpiry
			,@SubjectCode
			,@RollNumber
			,@FullName
			,@Nationality
			,@PlaceOfBirth
			,@Curriculum
			,@GraduationYear
			,@GraduationGrade
			,@GraduationDecisionNumber
			,@DiplomaNumber
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
