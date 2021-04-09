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
@CampusId					INT,
@SignatureId				INT
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
		   ,[SignatureId]
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
			,@SignatureId
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
		SELECT SCOPE_IDENTITY() 
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
@Gender				BIT,
@DOB				DATE,
@PhoneNumber		NVARCHAR(20),
@PersonalEmail		VARCHAR(50),
@AcademicEmail		VARCHAR(50),
@RollNumber			VARCHAR(50),
@MemberCode			VARCHAR(50),
@Ethnicity			NVARCHAR(50)
AS
BEGIN
		INSERT INTO [dbo].[User]
			   ([PasswordHash]
			   ,[Gender]
			   ,[DOB]
			   ,[PhoneNumber]
			   ,[PersonalEmail]
			   ,[AcademicEmail]
			   ,[RollNumber]
			   ,[MemberCode]
			   ,[Ethnicity]
			   )
		VALUES
			   (@PasswordHash
			   ,@Gender
			   ,@DOB
			   ,@PhoneNumber
			   ,@PersonalEmail
			   ,@AcademicEmail
			   ,@RollNumber
			   ,@MemberCode
			   ,@Ethnicity
			   )
			   SELECT SCOPE_IDENTITY() 
END
/*USER - UPDATE*/
CREATE PROCEDURE [dbo].[sp_Update_User]
@UserId				INT,
@PasswordHash		VARCHAR(200),
@Gender				BIT,
@DOB				DATE,
@PhoneNumber		NVARCHAR(20),
@PersonalEmail		VARCHAR(50),
@AcademicEmail		VARCHAR(50),
@RollNumber			VARCHAR(50),
@MemberCode			VARCHAR(50),
@Ethnicity			NVARCHAR(50)
AS
BEGIN
	UPDATE [dbo].[User]
	SET PasswordHash = @PasswordHash
		,Gender = @Gender
		,DOB = @DOB
		,PhoneNumber = @PhoneNumber
		,PersonalEmail = @PersonalEmail
		,AcademicEmail = @AcademicEmail
		,RollNumber = @RollNumber
		,MemberCode = @MemberCode
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
@CertificateId	INT,
@CreateTime		DATETIME,
@UpdateTime		DATETIME
AS
BEGIN
		INSERT INTO [dbo].[Report]
			   (
			   [ReportContent],
			   [Status],
			   [Title],
			   [UserId],
			   [CertificateId],
			   [CreateTime],
			   [UpdateTime]
			   )
		VALUES
			   (@ReportContent,
			    @Status,
			    @Title,
			    @UserId,
			    @CertificateId,
			    @CreateTime,
			    @UpdateTime
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
/*SIGNATURE - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Signature]
@FullName				NVARCHAR(200),
@Position				NVARCHAR(200),
@ImageFile				NVARCHAR(200)
AS
BEGIN
			INSERT INTO [dbo].[Signature]
					(
					[FullName],
					[Position],
					[ImageFile]
					)
			VALUES	
					(@FullName,
					@Position,
					@ImageFile
					)
					SELECT SCOPE_IDENTITY()
END
/*SIGNATURE_EDUCATIONSYSTEM - INSERT*/
CREATE PROCEDURE [dbo].[sp_Insert_Signature_EducationSystem]
@SignatureId			INT,
@EducationSystemId		INT
AS
BEGIN
			INSERT INTO [dbo].[Signature_EducationSystem]
					(
					[SignatureId],
					[EducationSystemId]
					)
			VALUES	
					(@SignatureId,
					@EducationSystemId
					)
					SELECT SCOPE_IDENTITY()
END
/*Insert Academic Service User*/
CREATE PROCEDURE [dbo].[sp_Insert_AcademicServiceUser]
@AcademicEmail		VARCHAR(50),
@PhoneNumber		NVARCHAR(20),
@CampusId			INT
AS
BEGIN
DECLARE @RoleName NVARCHAR(20)
SET @RoleName = 'Academic Service'
DECLARE @UserId INT
DECLARE @RoleId INT
			--Insert into [User]
			INSERT INTO [dbo].[User]
					(
					[AcademicEmail],
					[PhoneNumber]
					)
			VALUES	
					(
					@AcademicEmail,
					@PhoneNumber
					)
			SET @UserId = SCOPE_IDENTITY()
			--Insert into [Role]
			INSERT INTO [dbo].[Role]
					(
					[RoleName],
					[CampusId]
					)
			VALUES	
					(@RoleName,
					@CampusId
					)
			SET @RoleId = SCOPE_IDENTITY()
			--Insert to [User_Role]
			INSERT INTO [dbo].[User_Role]
					(
					[UserId],
					[RoleId]
					)
			VALUES	
					(@UserId,
					@RoleId
					)
END
CREATE PROCEDURE [dbo].[sp_Insert_AcademicServiceUser]

/*USER - DELETE ROLE ADMIN*/
CREATE PROCEDURE [dbo].sp_Delete_Role_Admin
@CampusId			INT
AS
BEGIN
		DELETE FROM [dbo].[Role]
		WHERE CampusId = @CampusId 
		AND RoleName = 'Admin'		
END

/*INSERT ADMIN*/
CREATE PROCEDURE [dbo].[sp_Insert_Existed_AdminUser]
@CampusId			INT,
@UserId				INT	
AS
BEGIN
DECLARE @RoleName NVARCHAR(20)
SET @RoleName = 'Admin'
DECLARE @RoleId INT
			
			--Insert into [Role]
			INSERT INTO [dbo].[Role]
					(
					[RoleName],
					[CampusId]
					)
			VALUES	
					(@RoleName,
					@CampusId
					)
			SET @RoleId = SCOPE_IDENTITY()
			--Insert to [User_Role]
			INSERT INTO [dbo].[User_Role]
					(
					[UserId],
					[RoleId]
					)
			VALUES	
					(@UserId,
					@RoleId
					)
END


CREATE PROCEDURE [dbo].[sp_Insert_AdminUser]
@AcademicEmail		VARCHAR(50),
@PhoneNumber		NVARCHAR(20),
@CampusId			INT
AS
BEGIN
DECLARE @RoleName NVARCHAR(20)
SET @RoleName = 'Admin'
DECLARE @UserId INT
DECLARE @RoleId INT
			--Insert into [User]
			INSERT INTO [dbo].[User]
					(
					[AcademicEmail],
					[PhoneNumber]
					)
			VALUES	
					(
					@AcademicEmail,
					@PhoneNumber
					)
			SET @UserId = SCOPE_IDENTITY()
			--Insert into [Role]
			INSERT INTO [dbo].[Role]
					(
					[RoleName],
					[CampusId]
					)
			VALUES	
					(@RoleName,
					@CampusId
					)
			SET @RoleId = SCOPE_IDENTITY()
			--Insert to [User_Role]
			INSERT INTO [dbo].[User_Role]
					(
					[UserId],
					[RoleId]
					)
			VALUES	
					(@UserId,
					@RoleId
					)
END



/*DELETE Campus*/
CREATE PROCEDURE [dbo].sp_Delete_Campus
@CampusId			INT
AS
BEGIN
		DELETE FROM [dbo].[Campus]
		WHERE CampusId = @CampusId 
END

select * from [User]
select * from [User_Role]
select * from Role


/*DROP STORE*/
DROP PROC sp_Insert_Certificate
DROP PROC sp_Insert_CertificateContent
DROP PROC sp_Insert_Certificate_User
DROP PROC [sp_Insert_User]
DROP PROC [sp_Update_User]
DROP PROC [sp_Insert_EducationSystem]

DROP PROC[sp_Insert_AcademicServiceUser]