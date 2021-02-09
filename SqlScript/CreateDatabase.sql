

CREATE TABLE [Organizations] (
  [OrganizationId] int NOT NULL IDENTITY(1,1),
  [OrganizationName] varchar(50),
  [LogoImage] varchar(20),
  PRIMARY KEY ([OrganizationId])
);

CREATE TABLE [UserLogs] (
  [UserLogId] int NOT NULL IDENTITY(1,1),
  [LoginTime] datetime,
  [IsSuccess] bit,
  [LogoutTime] datetime,
  [UserId] int,
  PRIMARY KEY ([UserLogId])
);

CREATE TABLE [Role_Permissions] (
  [RoleId] int NOT NULL IDENTITY(1,1),
  [PermissionId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([RoleId], [PermissionId])
);



CREATE TABLE [Reports] (
  [ReportId] int NOT NULL IDENTITY(1,1),
  [ReportContent] nvarchar(100),
  [Status] nvarchar(20),
  [UserId] int,
  [CertificateId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([ReportId])
);

CREATE TABLE [Certificate_Categories] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [CategoryId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([CertificateId], [CategoryId])
);



CREATE TABLE [Users] (
  [UserId] int NOT NULL IDENTITY(1,1),
  [PasswordHash] varchar(200),
  [PasswordSalt] varchar(100),
  [FirstName] nvarchar(20),
  [MiddleName] nvarchar(20),
  [LastName] nvarchar(20),
  [Gender] bit,
  [DOB] date,
  [IDCard] varchar(20),
  [Address] nvarchar(50),
  [PhoneNumber] nvarchar(20),
  [PersonalEmail] varchar(50),
  [AcademicEmail] varchar(50),
  [DateOfIssue] date,
  [PlaceOfIssue] nvarchar(50),
  [RollNumber] varchar(10),
  [OldRollNumber] varchar(10),
  [MemberCode] varchar(20),
  [EnrolDate] date,
  [Mode] varchar(20),
  [Status] varchar(20),
  [CurrentTermNo] int,
  [Major] varchar(20),
  [Curriculumn] varchar(20),
  [RoleId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([UserID])
);

CREATE TABLE [Permissions] (
  [PermissionId] int NOT NULL IDENTITY(1,1),
  [PermissionAction] varchar(100),
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([PermissionId])
);

CREATE TABLE [Portfolios] (
  [PortfolioId] int NOT NULL IDENTITY(1,1),
  [PortfolioName] varchar(50),
  [UserId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([PortfolioId])
);

CREATE TABLE [Roles] (
  [RoleId] int NOT NULL IDENTITY(1,1),
  [RoleName] varchar(20),
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([RoleId])
);

CREATE TABLE [Categories] (
  [CategoryId] int NOT NULL IDENTITY(1,1),
  [CategoryName] varchar(50),
  [UserID] int,
  [ExpiryDate] datetime,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([CategoryId])
);

CREATE TABLE [Portfolio_Certificates] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [PortfolioId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([CertificateId], [PortfolioId])
);


CREATE TABLE [Certificates] (
  [CertificateId] int NOT NULL IDENTITY(1,1),
  [CertificateName] nvarchar(50),
  [VerifyCode] varchar(20),
  [Issuer] varchar(20),
  [Description] nvarchar(200),
  [Hashing] varchar(200),
  [ViewCount] int,
  [DateOfIssue] datetime,
  [DateOfExpiry] datetime,
  [UserId] int,
  [OrganizationId] int,
  [created_at] datetime,
  [updated_at] datetime,
  PRIMARY KEY ([CertificateID])
);

CREATE TABLE Certificate_User(
	[UserId] int,
	[CertificateId] int,
	[created_at] datetime,
	[updated_at] datetime,
	PRIMARY KEY ([UserId], [CertificateId])
)

CREATE TABLE [CertificateContents](
	[CertificateContentId] int NOT NULL IDENTITY(1,1),
	[Content] varchar(200),
	[Format] varchar(20),
	[CertificateId] int,
	[created_at] datetime,
	[updated_at] datetime,
	PRIMARY KEY ([CertificateContentId])
)

CREATE TABLE [Transcripts](
	[TranscriptId] int NOT NULL IDENTITY(1,1),
	[Semester] varchar(50),
	[SubjectCode] varchar(20),
	[SubjectName] nvarchar(100),
	[Mark] float,
	[UserId] int,
	[created_at] datetime,
	[updated_at] datetime,
	PRIMARY KEY ([TranscriptId])
)


