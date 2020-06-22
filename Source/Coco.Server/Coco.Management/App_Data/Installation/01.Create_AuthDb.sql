--USER--
GO
CREATE TABLE dbo.[User]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Email NVARCHAR(255) NOT NULL,
	Lastname NVARCHAR(255) NOT NULL,
	FirstName NVARCHAR(255) NOT NULL,
	DisplayName NVARCHAR(255) NOT NULL,
	UserName NVARCHAR(255) NOT NULL,
	[PasswordHash] NVARCHAR(MAX) NOT NULL,
	IsEmailConfirmed BIT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NULL,
	CreatedById BIGINT NULL,
	IsActived BIT NOT NULL,
	StatusId TINYINT NOT NULL,
	SecurityStamp NVARCHAR(MAX) NOT NULL
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

--ROLE--
GO
CREATE TABLE dbo.[Role]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ConcurrencyStamp NVARCHAR(60) NULL
)

GO
ALTER TABLE dbo.[Role]
ADD CONSTRAINT PK_Role
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[Role]
ADD CONSTRAINT FK_Role_CreatedBy
FOREIGN KEY (CreatedById) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.[Role]
ADD CONSTRAINT FK_Role_UpdatedBy
FOREIGN KEY (UpdatedById) REFERENCES dbo.[User](Id);
--USER_ROLE--
GO
CREATE TABLE dbo.[UserRole]
(
	RoleId BIGINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
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

/**ROLE CLAIMS**/
GO
CREATE TABLE dbo.[RoleClaim]
(
	Id INT NOT NULL IDENTITY(1,1),
	RoleId BIGINT NOT NULL,
	ClaimType NVARCHAR(MAX) NOT NULL,
	ClaimValue NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE dbo.[RoleClaim]
ADD CONSTRAINT PK_RoleClaim
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[RoleClaim]
ADD CONSTRAINT FK_RoleClaim_User
FOREIGN KEY (RoleId) REFERENCES dbo.[Role](Id);
--[AuthorizationPolicy]--
GO
CREATE TABLE dbo.[AuthorizationPolicy]
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.[AuthorizationPolicy]
ADD CONSTRAINT PK_AuthorizationPolicy
PRIMARY KEY (Id);

--[UserAuthorizationPolicy]--
GO
CREATE TABLE dbo.UserAuthorizationPolicy
(
	AuthorizationPolicyId SMALLINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE dbo.UserAuthorizationPolicy
ADD CONSTRAINT FK_UserAuthorizationPolicy_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.[UserAuthorizationPolicy]
ADD CONSTRAINT FK_UserAuthorizationPolicy_AuthorizationPolicy
FOREIGN KEY (AuthorizationPolicyId) REFERENCES dbo.[AuthorizationPolicy](Id);

GO
ALTER TABLE dbo.[UserAuthorizationPolicy]
ADD CONSTRAINT PK_UserAuthorizationPolicy
PRIMARY KEY (AuthorizationPolicyId, UserId);

--[RoleAuthorizationPolicy]--
GO
CREATE TABLE dbo.RoleAuthorizationPolicy
(
	AuthorizationPolicyId SMALLINT NOT NULL,
	RoleId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE dbo.[RoleAuthorizationPolicy]
ADD CONSTRAINT FK_RoleAuthorizationPolicy_Role
FOREIGN KEY (RoleId) REFERENCES dbo.[Role](Id);

GO
ALTER TABLE dbo.[RoleAuthorizationPolicy]
ADD CONSTRAINT FK_RoleAuthorizationPolicy_AuthorizationPolicy
FOREIGN KEY (AuthorizationPolicyId) REFERENCES dbo.[AuthorizationPolicy](Id);

GO
ALTER TABLE dbo.[RoleAuthorizationPolicy]
ADD CONSTRAINT PK_RoleAuthorizationPolicy
PRIMARY KEY (AuthorizationPolicyId, RoleId);

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

/**USER ATTRIBUTE**/
GO
CREATE TABLE dbo.UserAttribute
(
	Id INT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	[Key] NVARCHAR(400) NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	Expiration DATETIME2 NULL,
	IsDisabled BIT NOT NULL
)

GO
ALTER TABLE dbo.UserAttribute
ADD CONSTRAINT PK_UserAttribute
PRIMARY KEY (Id);

/**USER CLAIMS**/
GO
CREATE TABLE dbo.[UserClaim]
(
	Id INT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	ClaimType NVARCHAR(MAX) NOT NULL,
	ClaimValue NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE dbo.[UserClaim]
ADD CONSTRAINT PK_UserClaim
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[UserClaim]
ADD CONSTRAINT FK_UserClaim_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

/**USER TOKENS**/
GO
CREATE TABLE dbo.[UserToken]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(255) NOT NULL,
	[LoginProvider] NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE dbo.[UserToken]
ADD CONSTRAINT FK_UserToken_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.[UserToken]
ADD CONSTRAINT PK_UserToken
PRIMARY KEY (Id);


/**USER LOGINS**/
GO
CREATE TABLE dbo.[UserLogin]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	LoginProvider NVARCHAR(255) NOT NULL,
	ProviderDisplayName NVARCHAR(255) NOT NULL,
	ProviderKey NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE dbo.[UserLogin]
ADD CONSTRAINT FK_UserLogin_User
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);

GO
ALTER TABLE dbo.[UserLogin]
ADD CONSTRAINT PK_UserLogin
PRIMARY KEY (Id);