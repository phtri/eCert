CREATE TABLE [Organization] (
  [OrganizationId] int NOT NULL IDENTITY(1,1),
  [OrganizationName] varchar(50),
  [LogoImage] varchar(20),
  PRIMARY KEY ([OrganizationId])
);

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
  [FirstName] nvarchar(20),
  [MiddleName] nvarchar(20),
  [LastName] nvarchar(20),
  [Gender] bit,
  [DOB] date,
  [PhoneNumber] nvarchar(20),
  [PersonalEmail] varchar(50),
  [AcademicEmail] varchar(50),
  [RollNumber] varchar(10),
  [RoleId] int,
  UNIQUE([RollNumber]),
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
  PRIMARY KEY ([RoleId])
);

CREATE TABLE [Categorie] (
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
  [VerifyCode] varchar(20),
  [Issuer] varchar(20),
  [Description] nvarchar(200),
  [Hashing] varchar(200),
  [ViewCount] int,
  [DateOfIssue] date,
  [DateOfExpiry] date,
  [SubjectCode] varchar(50),
  [OrganizationId] int,
  PRIMARY KEY ([CertificateID])
);

CREATE TABLE Certificate_User(
	[RollNumber] varchar(10),
	[CertificateId] int,
	PRIMARY KEY ([RollNumber], [CertificateId])
)

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