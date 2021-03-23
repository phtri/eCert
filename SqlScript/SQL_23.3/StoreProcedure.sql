/*CERTIFICATES - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificate]
@CertificateName			NVARCHAR(50),
@VerifyCode					NVARCHAR(100),
@Url						VARCHAR(100),
@IssuerType					VARCHAR(20),
@IssuerName					NVARCHAR(200),
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
@CampusId					INT
AS
BEGIN
		INSERT INTO [dbo].[Certificate]
           ([CertificateName]
           ,[VerifyCode]
		   ,[Url]
           ,[IssuerType]
		   ,[IssuerName]
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
		   ,[CampusId]
           )
		VALUES
           (@CertificateName		
			,@VerifyCode				
			,@Url
			,@IssuerType
			,@IssuerName
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
			,@CampusId
           )
		   SELECT SCOPE_IDENTITY() 
END

/*CERTIFICATES - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_Certificate]
@CertificateId	INT
AS
BEGIN	
	DELETE FROM [dbo].[Certificate]
	WHERE CertificateId = @CertificateId
END
/*CertificateContent - INSERT*/
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
/*CertificateContent - Delete*/
CREATE PROCEDURE [dbo].[sp_Delete_CertificateContent]
@CertificateId	INT
AS
BEGIN	
	DELETE FROM [dbo].[CertificateContent]
	WHERE CertificateId = @CertificateId
END
/*EducationSystem - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_EducationSystem]
@EducationName			NVARCHAR(100),
@LogoImage				VARCHAR(100)
AS
BEGIN
		INSERT INTO EducationSystem(
			EducationName,
			LogoImage
			)
		VALUES(@EducationName, @LogoImage)
END
/*EducationSystem - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_EducationSystem]
@EducationSystemId		INT
AS
BEGIN
		DELETE FROM [dbo].[EducationSystem]
		WHERE EducationSystemId = @EducationSystemId
END

/*CERTIFICATE_USER - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Certificate_User]
@UserId					INT,
@CertificateId			INT
AS
BEGIN
		INSERT INTO Certificate_User(
			UserId,
			CertificateId
			)
		VALUES(@UserId, @CertificateId)
END
/*CERTIFICATE_USER - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_Certificate_User]
@CertificateId			INT
AS
BEGIN
		DELETE FROM [dbo].[Certificate_User]
		WHERE CertificateId = @CertificateId
END
/*USER - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_User]
@PasswordHash		VARCHAR(200),
@PasswordSalt		VARCHAR(100),
@Gender				BIT,
@DOB				DATE,
@PhoneNumber		NVARCHAR(20),
@PersonalEmail		VARCHAR(50),
@AcademicEmail		VARCHAR(50),
@RollNumber			VARCHAR(50),
@Ethnicity			NVARCHAR(50)
AS
BEGIN
		INSERT INTO [dbo].[User]
			   ([PasswordHash]
			   ,[PasswordSalt]
			   ,[Gender]
			   ,[DOB]
			   ,[PhoneNumber]
			   ,[PersonalEmail]
			   ,[AcademicEmail]
			   ,[RollNumber]
			   ,[Ethnicity]
			   )
		VALUES
			   (@PasswordHash
			   ,@PasswordSalt
			   ,@Gender
			   ,@DOB
			   ,@PhoneNumber
			   ,@PersonalEmail
			   ,@AcademicEmail
			   ,@RollNumber	
			   ,@Ethnicity
			   )
			   SELECT SCOPE_IDENTITY() 
END
/*USER - UPDATE*/
CREATE PROCEDURE [dbo].[sp_Update_User]
@UserId				INT,
@PasswordHash		VARCHAR(200),
@PasswordSalt		VARCHAR(100),
@Gender				BIT,
@DOB				DATE,
@PhoneNumber		NVARCHAR(20),
@PersonalEmail		VARCHAR(50),
@AcademicEmail		VARCHAR(50),
@RollNumber			VARCHAR(50),
@Ethnicity			NVARCHAR(50)
AS
BEGIN
	UPDATE [dbo].[User]
	SET PasswordHash = @PasswordHash
		,PasswordSalt = @PasswordSalt
		,Gender = @Gender
		,DOB = @DOB
		,PhoneNumber = @PhoneNumber
		,PersonalEmail = @PersonalEmail
		,AcademicEmail = @AcademicEmail
		,RollNumber = @RollNumber
		,Ethnicity = @Ethnicity
	WHERE
		UserId = @UserId
END
/*USER - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_User]
@UserId			INT
AS
BEGIN
		DELETE FROM [dbo].[User]
		WHERE UserId = @UserId
END
/*USER_ROLE - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_User_Role]
@UserId			INT,
@RoleId			INT
AS
BEGIN
		INSERT INTO [dbo].[User_Role]
			   (
					[UserId],
					[RoleId]
			   )
		VALUES
			   (@UserId,
			   @RoleId
			   )
			   SELECT SCOPE_IDENTITY() 
END
/*USER_ROLE - DELETE*/
CREATE PROCEDURE [dbo].[sp_Delete_User_Role]
@UserId			INT
AS
BEGIN
		DELETE FROM [dbo].[User_Role]
		WHERE UserId = @UserId
END

/*REPORT - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Report]
@ReportContent	NVARCHAR(100),
@Status			NVARCHAR(20),
@Title			NVARCHAR(100),
@UserId			INT,
@CertificateId	INT
AS
BEGIN
		INSERT INTO [dbo].[Report]
			   (
			   [ReportContent],
			   [Status],
			   [Title],
			   [UserId],
			   [CertificateId]
			   )
		VALUES
			   (@ReportContent,
			   @Status,
			   @Title,
			   @UserId,
			   @CertificateId
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


/*DROP STORE*/
DROP PROC sp_Insert_Certificate
DROP PROC sp_Insert_CertificateContent
DROP PROC sp_Insert_Certificate_User
DROP PROC [sp_Insert_User]