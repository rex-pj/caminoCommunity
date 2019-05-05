CREATE DATABASE CocoUserDb

GO
USE CocoUserDb


GO
CREATE SCHEMA Account

--USER--
GO
CREATE TABLE Account.[User]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Lastname NVARCHAR(255) NOT NULL,
	FirstName NVARCHAR(255) NOT NULL,
	DisplayName NVARCHAR(255) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
	[Password] NVARCHAR(255) NOT NULL,
	PasswordSalt NVARCHAR(255) NOT NULL
)

GO
ALTER TABLE Account.[User]
ADD CONSTRAINT PK_User
PRIMARY KEY (Id);

--USER INFO--
GO
CREATE TABLE Account.UserInfo
(
	Id BIGINT NOT NULL,
	PhoneNumber VARCHAR(50) NULL,
	[Address] NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	BirthDate DATETIME2 NULL,
	CreatedDate DATETIME2 NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NULL,
	CreatedById BIGINT NULL,
	GenderId TINYINT NULL,
	CountryId SMALLINT NULL,
	IsActived BIT NOT NULL,
	StatusId TINYINT NOT NULL
)


GO
ALTER TABLE Account.UserInfo
ADD CONSTRAINT PK_UserInfo
PRIMARY KEY (Id);

GO
ALTER TABLE Account.[UserInfo]
ADD CONSTRAINT FK_UserInfo_User
FOREIGN KEY (Id) REFERENCES Account.[User](Id);

-- USER STATUS--
CREATE TABLE Account.[Status]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE Account.[Status]
ADD CONSTRAINT PK_Status
PRIMARY KEY (Id);

GO
ALTER TABLE Account.[UserInfo]
ADD CONSTRAINT FK_UserInfo_Status
FOREIGN KEY (StatusId) REFERENCES Account.[Status](Id);
--  --
GO
CREATE SCHEMA Work

GO
CREATE TABLE Work.Career
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
ALTER TABLE Work.Career
ADD CONSTRAINT PK_Career
PRIMARY KEY (Id);

GO
ALTER TABLE Work.Career
ADD CONSTRAINT FK_Career_CreatedBy
FOREIGN KEY (CreatedById) REFERENCES Account.[User](Id);

GO
ALTER TABLE Work.Career
ADD CONSTRAINT FK_Career_UpdatedBy
FOREIGN KEY (UpdatedById) REFERENCES Account.[User](Id);
-- USER BUSINESS --
GO
CREATE TABLE Work.UserCareer
(
	CareerId TINYINT NOT NULL,
	UserId BIGINT NOT NULL
)

GO
ALTER TABLE Work.UserCareer
ADD CONSTRAINT FK_UserCareer_User
FOREIGN KEY (UserId) REFERENCES Account.[User](Id);

GO
ALTER TABLE Work.UserCareer
ADD CONSTRAINT FK_UserCareer_Career
FOREIGN KEY (CareerId) REFERENCES Work.Career(Id);

GO
ALTER TABLE Work.UserCareer
ADD CONSTRAINT PK_UserCareer
PRIMARY KEY (CareerId, UserId);

/**CREATE AUTH SCHEMA**/
GO
CREATE SCHEMA Auth;
--ROLE--
GO
CREATE TABLE Auth.[Role]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE Auth.[Role]
ADD CONSTRAINT PK_Role
PRIMARY KEY (Id);

--USER_ROLE--
GO
CREATE TABLE Auth.[UserRole]
(
	RoleId TINYINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE Auth.[UserRole]
ADD CONSTRAINT FK_UserRole_User
FOREIGN KEY (UserId) REFERENCES Account.[User](Id);

GO
ALTER TABLE Auth.[UserRole]
ADD CONSTRAINT FK_UserRole_Role
FOREIGN KEY (RoleId) REFERENCES Auth.[Role](Id);

GO
ALTER TABLE Auth.[UserRole]
ADD CONSTRAINT PK_UserRole
PRIMARY KEY (RoleId, UserId);


--GENDER--
CREATE TABLE Account.Gender
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(10)
)

GO
ALTER TABLE Account.Gender
ADD CONSTRAINT PK_Gender
PRIMARY KEY (Id);

--COUNTRY--
GO
CREATE TABLE dbo.Country
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	Code VARCHAR(5) NOT NULL,
	Name NVARCHAR(10) NOT NULL
)

GO
ALTER TABLE dbo.Country
ADD CONSTRAINT PK_Country
PRIMARY KEY (Id);

/**FOREIGN KEY**/
GO
ALTER TABLE Account.[UserInfo]
ADD CONSTRAINT FK_UserInfo_Gender
FOREIGN KEY (GenderId) REFERENCES Account.Gender(Id);

GO
ALTER TABLE Account.[UserInfo]
ADD CONSTRAINT FK_UserInfo_Country
FOREIGN KEY (CountryId) REFERENCES dbo.Country(Id);



