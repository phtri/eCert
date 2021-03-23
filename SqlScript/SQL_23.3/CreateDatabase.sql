CREATE TABLE [UserLog] (
  [UserLogId] int NOT NULL IDENTITY(1,1),
  [LoginTime] datetime,
  [IsSuccess] bit,
  [LogoutTime] datetime,
  [UserId] int,
  PRIMARY KEY ([UserLogId])
);

CREATE TABLE [Report] (
  [ReportId] int NOT NULL IDENTITY(1,1),
  [ReportContent] nvarchar(100),
  [Status] nvarchar(20),
  [Title] nvarchar(100)
  [UserId] int,
  [CertificateId] int,
  PRIMARY KEY ([ReportId])
);

CREATE TABLE [Certificate_Categorie] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [CategoryId] int,
  PRIMARY KEY ([CertificateId], [CategoryId])
);

CREATE TABLE [User] (
  [UserId] int NOT NULL IDENTITY(1,1),
  [PasswordHash] varchar(200),
  [PasswordSalt] varchar(100),
  [Gender] bit,
  [DOB] date,
  [PhoneNumber] nvarchar(20),
  [PersonalEmail] varchar(50),
  [AcademicEmail] varchar(50),
  [RollNumber] varchar(50),
  [Ethnicity] nvarchar(50),
  PRIMARY KEY ([UserID])
);

CREATE TABLE [Portfolio] (
  [PortfolioId] int NOT NULL IDENTITY(1,1),
  [PortfolioName] varchar(50),
  [UserId] int,
  PRIMARY KEY ([PortfolioId])
);

CREATE TABLE [Role] (
  [RoleId] int NOT NULL IDENTITY(1,1),
  [RoleName] varchar(20),
  [CampusId] int,
  PRIMARY KEY ([RoleId])
);

CREATE TABLE [Category] (
  [CategoryId] int NOT NULL IDENTITY(1,1),
  [CategoryName] varchar(50),
  [UserID] int,
  [ExpiryDate] datetime,
  PRIMARY KEY ([CategoryId])
);

CREATE TABLE [Portfolio_Certificate] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [PortfolioId] int,
  PRIMARY KEY ([CertificateId], [PortfolioId])
);

CREATE TABLE [Certificate] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [CertificateName] nvarchar(50),
  [VerifyCode] nvarchar(100),
  [Url] varchar(100),
  [IssuerType] varchar(20),
  [IssuerName] nvarchar(200),
  [Description] nvarchar(200),
  [Hashing] varchar(200),
  [ViewCount] int,
  [DateOfIssue] date,
  [DateOfExpiry] date,
  [SubjectCode] varchar(50),
  [RollNumber] varchar(50),
  [FullName] nvarchar(100),
  [Nationality] nvarchar(100),
  [PlaceOfBirth] nvarchar(100),
  [Curriculum] varchar(50),
  [GraduationYear] date,
  [GraduationGrade] nvarchar(100),
  [GraduationDecisionNumber] nvarchar(100),
  [DiplomaNumber] nvarchar(100),
  [CampusId] int,
  PRIMARY KEY ([CertificateID])
);

CREATE TABLE [CertificateContent](
	[CertificateContentId] int NOT NULL IDENTITY(1,1),
	[Content] varchar(200),
	[CertificateFormat] varchar(20),
	[CertificateId] int,
	PRIMARY KEY ([CertificateContentId])
)

-- 01/March/2021 - TriHP
CREATE TABLE [User_Role](
	[UserId] int,
	[RoleId] int,
	PRIMARY KEY ([UserId], [RoleId])
)
-- 15/March/2021 - TriHP
CREATE TABLE [EducationSystem] (
  [EducationSystemId] int NOT NULL IDENTITY(1,1),
  [EducationName] varchar(100),
  [LogoImage] varchar(100),
  PRIMARY KEY ([EducationSystemId])
);

CREATE TABLE [Campus] (
  [CampusId] int NOT NULL IDENTITY(1,1),
  [CampusName] varchar(100),
  [EducationSystemId] int,
  PRIMARY KEY ([CampusId])
);
-- 23/March/2021 - TriHP
CREATE TABLE [Signature] (
  [SignatureId] int NOT NULL IDENTITY(1,1),
  [FullName] nvarchar(200),
  [Postion] nvarchar(200),
  [ImageFile] varchar(200),
  PRIMARY KEY ([SignatureId])
);
CREATE TABLE [Signature_EducationSystem] (
	[SignatureId] int,
	[EducationSystemId] int,
	PRIMARY KEY ([SignatureId], [EducationSystemId])
);

/*
CREATE TABLE [Role_Permission] (
  [RoleId] int NOT NULL IDENTITY(1,1),
  [PermissionId] int,
  PRIMARY KEY ([RoleId], [PermissionId])
);
*/

/*
CREATE TABLE [Permission] (
  [PermissionId] int NOT NULL IDENTITY(1,1),
  [PermissionAction] varchar(100),
  PRIMARY KEY ([PermissionId])
);
*/