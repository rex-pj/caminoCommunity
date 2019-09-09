CREATE DATABASE Coco_IdentityDb

GO
USE Coco_IdentityDb;

--USER--
GO
CREATE TABLE dbo.[User]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Email NVARCHAR(255) NOT NULL,
	Lastname NVARCHAR(255) NOT NULL,
	FirstName NVARCHAR(255) NOT NULL,
	DisplayName NVARCHAR(255) NOT NULL,
	[Password] NVARCHAR(MAX) NOT NULL,
	PasswordSalt NVARCHAR(MAX) NOT NULL,
	SecurityStamp NVARCHAR(MAX) NULL,
	IsEmailConfirmed BIT NOT NULL,
	AuthenticatorToken NVARCHAR(MAX) NULL,
	Expiration DATETIME2 NULL,
	CreatedDate DATETIME2 NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NULL,
	CreatedById BIGINT NULL,
	IsActived BIT NOT NULL,
	IdentityStamp NVARCHAR(MAX) NULL,
	StatusId TINYINT NOT NULL,
)

GO
ALTER TABLE dbo.[User]
ADD CONSTRAINT PK_User
PRIMARY KEY (Id);

--USER INFO--
GO
CREATE TABLE dbo.UserInfo
(
	Id BIGINT NOT NULL,
	PhoneNumber VARCHAR(50) NULL,
	[Address] NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	BirthDate DATETIME2 NULL,
	GenderId TINYINT NULL,
	CountryId SMALLINT NULL,
	AvatarUrl NVARCHAR(MAX) NULL,
	CoverPhotoUrl NVARCHAR(MAX) NULL
)

GO
ALTER TABLE dbo.UserInfo
ADD CONSTRAINT PK_UserInfo
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[UserInfo]
ADD CONSTRAINT FK_UserInfo_User
FOREIGN KEY (Id) REFERENCES dbo.[User](Id);

-- USER PHOTO --
GO 
CREATE TABLE dbo.UserPhotoType
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000)
)

GO
ALTER TABLE dbo.UserPhotoType
ADD CONSTRAINT PK_UserPhotoType
PRIMARY KEY (Id);

GO
CREATE TABLE dbo.UserPhoto(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Code NVARCHAR(MAX) NOT NULL,
	[Name] NVARCHAR(255) NULL,
	[Url] NVARCHAR(2000) NULL,
	[Description] NVARCHAR(1000) NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ImageData NVARCHAR(MAX) NOT NULL,
	UserId BIGINT NOT NULL,
	TypeId TINYINT NOT NULL
)

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT PK_UserPhoto
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT FK_UserPhoto_UserPhotoType
FOREIGN KEY (TypeId) REFERENCES dbo.UserPhotoType(Id);

-- USER STATUS--
CREATE TABLE dbo.[Status]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[Status]
ADD CONSTRAINT PK_Status
PRIMARY KEY (Id);


GO
ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_Status
FOREIGN KEY (StatusId) REFERENCES dbo.[Status](Id);

GO
CREATE TABLE dbo.Career
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(10),
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.Career
ADD CONSTRAINT PK_Career
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.Career
ADD CONSTRAINT FK_Career_CreatedBy
FOREIGN KEY (CreatedById) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.Career
ADD CONSTRAINT FK_Career_UpdatedBy
FOREIGN KEY (UpdatedById) REFERENCES dbo.[User](Id);

-- USER BUSINESS --
GO
CREATE TABLE dbo.UserCareer
(
	CareerId TINYINT NOT NULL,
	UserId BIGINT NOT NULL
)

GO
ALTER TABLE dbo.UserCareer
ADD CONSTRAINT FK_UserCareer_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.UserCareer
ADD CONSTRAINT FK_UserCareer_Career
FOREIGN KEY (CareerId) REFERENCES dbo.Career(Id);

GO
ALTER TABLE dbo.UserCareer
ADD CONSTRAINT PK_UserCareer
PRIMARY KEY (CareerId, UserId);

--ROLE--
GO
CREATE TABLE dbo.[Role]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[Role]
ADD CONSTRAINT PK_Role
PRIMARY KEY (Id);

--USER_ROLE--
GO
CREATE TABLE dbo.[UserRole]
(
	RoleId TINYINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE dbo.[UserRole]
ADD CONSTRAINT FK_UserRole_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.[UserRole]
ADD CONSTRAINT FK_UserRole_Role
FOREIGN KEY (RoleId) REFERENCES dbo.[Role](Id);

GO
ALTER TABLE dbo.[UserRole]
ADD CONSTRAINT PK_UserRole
PRIMARY KEY (RoleId, UserId);


--GENDER--
CREATE TABLE dbo.Gender
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(10)
)

GO
ALTER TABLE dbo.Gender
ADD CONSTRAINT PK_Gender
PRIMARY KEY (Id);

--COUNTRY--
GO
CREATE TABLE dbo.Country
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	Code VARCHAR(5) NOT NULL,
	[Name] NVARCHAR(55) NOT NULL
)

GO
ALTER TABLE dbo.Country
ADD CONSTRAINT PK_Country
PRIMARY KEY (Id);

/**FOREIGN KEY**/
GO
ALTER TABLE dbo.[UserInfo]
ADD CONSTRAINT FK_UserInfo_Gender
FOREIGN KEY (GenderId) REFERENCES dbo.Gender(Id);

GO
ALTER TABLE dbo.[UserInfo]
ADD CONSTRAINT FK_UserInfo_Country
FOREIGN KEY (CountryId) REFERENCES dbo.Country(Id);