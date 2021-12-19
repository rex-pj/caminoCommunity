/** -----IDENTITY DATABASE---- **/
CREATE SCHEMA auth

-- USER STATUS--
GO
CREATE TABLE [auth].[Status]
(
	Id INT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE [auth].[Status]
ADD CONSTRAINT PK_Status
PRIMARY KEY (Id);

--USER--
GO
CREATE TABLE [auth].[User]
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
	StatusId INT NOT NULL,
	SecurityStamp NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE [auth].[User]
ADD CONSTRAINT PK_User
PRIMARY KEY (Id);

GO
ALTER TABLE [auth].[User]
ADD CONSTRAINT FK_User_Status
FOREIGN KEY (StatusId) REFERENCES [auth].[Status](Id);
--USER INFO--
GO
CREATE TABLE [auth].UserInfo
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
ALTER TABLE [auth].UserInfo
ADD CONSTRAINT PK_UserInfo
PRIMARY KEY (Id);

GO
ALTER TABLE [auth].[UserInfo]
ADD CONSTRAINT FK_UserInfo_User
FOREIGN KEY (Id) REFERENCES [auth].[User](Id);

--ROLE--
GO
CREATE TABLE [auth].[Role]
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
ALTER TABLE [auth].[Role]
ADD CONSTRAINT PK_Role
PRIMARY KEY (Id);

GO
ALTER TABLE [auth].[Role]
ADD CONSTRAINT FK_Role_CreatedBy
FOREIGN KEY (CreatedById) REFERENCES [auth].[User](Id);

GO
ALTER TABLE [auth].[Role]
ADD CONSTRAINT FK_Role_UpdatedBy
FOREIGN KEY (UpdatedById) REFERENCES [auth].[User](Id);
--USER_ROLE--
GO
CREATE TABLE [auth].[UserRole]
(
	RoleId BIGINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE [auth].[UserRole]
ADD CONSTRAINT FK_UserRole_User
FOREIGN KEY (UserId) REFERENCES [auth].[User](Id);

GO
ALTER TABLE [auth].[UserRole]
ADD CONSTRAINT FK_UserRole_Role
FOREIGN KEY (RoleId) REFERENCES [auth].[Role](Id);

GO
ALTER TABLE [auth].[UserRole]
ADD CONSTRAINT PK_UserRole
PRIMARY KEY (RoleId, UserId);

/**ROLE CLAIMS**/
GO
CREATE TABLE [auth].[RoleClaim]
(
	Id INT NOT NULL IDENTITY(1,1),
	RoleId BIGINT NOT NULL,
	ClaimType NVARCHAR(MAX) NOT NULL,
	ClaimValue NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE [auth].[RoleClaim]
ADD CONSTRAINT PK_RoleClaim
PRIMARY KEY (Id);

GO
ALTER TABLE [auth].[RoleClaim]
ADD CONSTRAINT FK_RoleClaim_User
FOREIGN KEY (RoleId) REFERENCES [auth].[Role](Id);
--[AuthorizationPolicy]--
GO
CREATE TABLE [auth].[AuthorizationPolicy]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE [auth].[AuthorizationPolicy]
ADD CONSTRAINT PK_AuthorizationPolicy
PRIMARY KEY (Id);

--[UserAuthorizationPolicy]--
GO
CREATE TABLE [auth].UserAuthorizationPolicy
(
	AuthorizationPolicyId BIGINT NOT NULL,
	UserId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE [auth].UserAuthorizationPolicy
ADD CONSTRAINT FK_UserAuthorizationPolicy_User
FOREIGN KEY (UserId) REFERENCES [auth].[User](Id);

GO
ALTER TABLE [auth].[UserAuthorizationPolicy]
ADD CONSTRAINT FK_UserAuthorizationPolicy_AuthorizationPolicy
FOREIGN KEY (AuthorizationPolicyId) REFERENCES [auth].[AuthorizationPolicy](Id);

GO
ALTER TABLE [auth].[UserAuthorizationPolicy]
ADD CONSTRAINT PK_UserAuthorizationPolicy
PRIMARY KEY (AuthorizationPolicyId, UserId);

--[RoleAuthorizationPolicy]--
GO
CREATE TABLE [auth].RoleAuthorizationPolicy
(
	AuthorizationPolicyId BIGINT NOT NULL,
	RoleId BIGINT NOT NULL,
	GrantedDate DATETIME2 NULL,
	GrantedById BIGINT NOT NULL,
	IsGranted BIT NOT NULL
)

GO
ALTER TABLE [auth].[RoleAuthorizationPolicy]
ADD CONSTRAINT FK_RoleAuthorizationPolicy_Role
FOREIGN KEY (RoleId) REFERENCES [auth].[Role](Id);

GO
ALTER TABLE [auth].[RoleAuthorizationPolicy]
ADD CONSTRAINT FK_RoleAuthorizationPolicy_AuthorizationPolicy
FOREIGN KEY (AuthorizationPolicyId) REFERENCES [auth].[AuthorizationPolicy](Id);

GO
ALTER TABLE [auth].[RoleAuthorizationPolicy]
ADD CONSTRAINT PK_RoleAuthorizationPolicy
PRIMARY KEY (AuthorizationPolicyId, RoleId);

--GENDER--
CREATE TABLE [auth].Gender
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(10)
)

GO
ALTER TABLE [auth].Gender
ADD CONSTRAINT PK_Gender
PRIMARY KEY (Id);

--COUNTRY--
GO
CREATE TABLE [auth].Country
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	Code VARCHAR(5) NOT NULL,
	[Name] NVARCHAR(55) NOT NULL
)

GO
ALTER TABLE [auth].Country
ADD CONSTRAINT PK_Country
PRIMARY KEY (Id);

/**FOREIGN KEY**/
GO
ALTER TABLE [auth].[UserInfo]
ADD CONSTRAINT FK_UserInfo_Gender
FOREIGN KEY (GenderId) REFERENCES [auth].Gender(Id);

GO
ALTER TABLE [auth].[UserInfo]
ADD CONSTRAINT FK_UserInfo_Country
FOREIGN KEY (CountryId) REFERENCES [auth].Country(Id);

/**USER ATTRIBUTE**/
GO
CREATE TABLE [auth].UserAttribute
(
	Id INT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	[Key] NVARCHAR(400) NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	Expiration DATETIME2 NULL,
	IsDisabled BIT NOT NULL
)

GO
ALTER TABLE [auth].UserAttribute
ADD CONSTRAINT PK_UserAttribute
PRIMARY KEY (Id);

/**USER CLAIMS**/
GO
CREATE TABLE [auth].[UserClaim]
(
	Id INT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	ClaimType NVARCHAR(MAX) NOT NULL,
	ClaimValue NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE [auth].[UserClaim]
ADD CONSTRAINT PK_UserClaim
PRIMARY KEY (Id);

GO
ALTER TABLE [auth].[UserClaim]
ADD CONSTRAINT FK_UserClaim_User
FOREIGN KEY (UserId) REFERENCES [auth].[User](Id);

/**USER TOKENS**/
GO
CREATE TABLE [auth].[UserToken]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(255) NOT NULL,
	[LoginProvider] NVARCHAR(MAX) NOT NULL,
	[ExpiryTime] DATETIME2(7) NOT NULL
)

GO
ALTER TABLE [auth].[UserToken]
ADD CONSTRAINT FK_UserToken_User
FOREIGN KEY (UserId) REFERENCES [auth].[User](Id);

GO
ALTER TABLE [auth].[UserToken]
ADD CONSTRAINT PK_UserToken
PRIMARY KEY (Id);


/**USER LOGINS**/
GO
CREATE TABLE [auth].[UserLogin]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	LoginProvider NVARCHAR(255) NOT NULL,
	ProviderDisplayName NVARCHAR(255) NOT NULL,
	ProviderKey NVARCHAR(MAX) NOT NULL
)

GO
ALTER TABLE [auth].[UserLogin]
ADD CONSTRAINT FK_UserLogin_User
FOREIGN KEY (UserId) REFERENCES [auth].[User](Id);

GO
ALTER TABLE [auth].[UserLogin]
ADD CONSTRAINT PK_UserLogin
PRIMARY KEY (Id);


/** -----CONTENT DATABASE---- **/
CREATE TABLE dbo.Menu
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Url] NVARCHAR(2000) NOT NULL,
	[Description] NVARCHAR(1000),
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentMenuId SMALLINT NULL,
	IsDeleted BIT NOT NULL,
	IsPublished BIT NOT NULL,
)

GO
ALTER TABLE dbo.Menu
ADD CONSTRAINT PK_Menu
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.Menu
ADD CONSTRAINT FK_Menu_ParentMenu
FOREIGN KEY (ParentMenuId) REFERENCES dbo.Menu(Id);

--GROUP--
GO
CREATE TABLE dbo.[Community](
	Id BIGINT NOT NULL IDENTITY(1,1),
	Title NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	IsDeleted BIT NOT NULL,
	IsPublished BIT NOT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.[Community]
ADD CONSTRAINT PK_Community
PRIMARY KEY (Id);

--GROUP ROLE--
GO
CREATE TABLE dbo.[CommunityRole]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[CommunityRole]
ADD CONSTRAINT PK_CommunityRole
PRIMARY KEY (Id);

--FARMER GROUP--
GO
CREATE TABLE dbo.CommunityMember(
	UserId BIGINT NOT NULL,
	CommunityId BIGINT NOT NULL,
	JoinedDate DATETIME2 NULL,
	IsJoined BIT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.CommunityMember
ADD CONSTRAINT PK_UserCommunity
PRIMARY KEY (UserId, CommunityId);

GO
ALTER TABLE dbo.CommunityMember
ADD CONSTRAINT FK_User_Community
FOREIGN KEY (CommunityId) REFERENCES dbo.[Community](Id);

--FARM TYPE--
GO
CREATE TABLE dbo.FarmType
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.FarmType
ADD CONSTRAINT PK_FarmType
PRIMARY KEY (Id)
--FARM--
GO
CREATE TABLE [dbo].[Farm]
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NULL,
	[Description] NVARCHAR(MAX) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	FarmTypeId BIGINT NOT NULL,
	[Address] NVARCHAR(500) NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.Farm
ADD CONSTRAINT PK_Farm
PRIMARY KEY (Id)

GO
ALTER TABLE dbo.Farm
ADD CONSTRAINT FK_Farm_FarmType
FOREIGN KEY (FarmTypeId) REFERENCES dbo.[FarmType](Id)
-- FARM ROLE --
GO
CREATE TABLE dbo.[FarmRole]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[FarmRole]
ADD CONSTRAINT PK_FarmRole
PRIMARY KEY (Id);

--User FARM--
GO
CREATE TABLE dbo.FarmMember(
	Id BIGINT NOT NULL IDENTITY(1,1),
	UserId BIGINT NOT NULL,
	FarmId BIGINT NOT NULL,
	JoinedDate DATETIME2 NULL,
	IsJoined BIT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.FarmMember
ADD CONSTRAINT PK_UserFarm
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.FarmMember
ADD CONSTRAINT FK_User_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);

--GROUP FARM--
GO
CREATE TABLE dbo.FarmCommunity(
	Id BIGINT NOT NULL IDENTITY(1,1),
	CommunityId BIGINT NOT NULL,
	FarmId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.FarmCommunity
ADD CONSTRAINT PK_FarmCommunity
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.FarmCommunity
ADD CONSTRAINT FK_FarmCommunity_Community
FOREIGN KEY (CommunityId) REFERENCES dbo.[Community](Id);

GO
ALTER TABLE dbo.FarmCommunity
ADD CONSTRAINT FK_FarmCommunity_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);
-- PRODUCT CATEGORY --
GO
CREATE TABLE dbo.[ProductCategory]
(
	Id INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentId INT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.[ProductCategory]
ADD CONSTRAINT PK_ProductCategory
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[ProductCategory]
ADD CONSTRAINT FK_ProductCategory_ParentProductCategory
FOREIGN KEY (ParentId) REFERENCES dbo.[ProductCategory](Id);

--PRODUCT--
GO
CREATE TABLE dbo.Product(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.[Product]
ADD CONSTRAINT PK_Product
PRIMARY KEY (Id);

-- PRODUCT CATEGORY RELATION --
GO
CREATE TABLE dbo.ProductCategoryRelation
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	ProductId BIGINT NOT NULL,
	ProductCategoryId INT NOT NULL
)

GO
ALTER TABLE dbo.ProductCategoryRelation
ADD CONSTRAINT FK_ProductCategoryRelation_ProductCategory
FOREIGN KEY (ProductCategoryId) REFERENCES dbo.[ProductCategory](Id);

GO
ALTER TABLE dbo.ProductCategoryRelation
ADD CONSTRAINT FK_ProductCategoryRelation_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.ProductCategoryRelation
ADD CONSTRAINT PK_ProductCategoryRelation
PRIMARY KEY (Id);

-- ORDER --
CREATE TABLE [dbo].[Order](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[CustomOrderNumber] [NVARCHAR](max) NOT NULL,
	[BillingAddress] [INT] NOT NULL,
	[CustomerId] [INT] NOT NULL,
	[PickupAddress] [INT] NULL,
	[ShippingAddress] [INT] NULL,
	[OrderGuid] [UNIQUEIDENTIFIER] NOT NULL,
	[StoreId] [INT] NOT NULL,
	[IsPickupInStore] [BIT] NOT NULL,
	[OrderStatusId] [INT] NOT NULL,
	[ShippingStatusId] [INT] NOT NULL,
	[PaymentStatusId] [INT] NOT NULL,
	[OrderDiscount] [DECIMAL](18, 4) NOT NULL,
	[OrderTotal] [DECIMAL](18, 4) NOT NULL,
	[RefundedAmount] [DECIMAL](18, 4) NOT NULL,
	[CustomerIp] [NVARCHAR](max) NULL,
	[PaidDateUtc] [datetime2](7) NULL,
	[ShippingMethod] [NVARCHAR](max) NULL,
	[IsDeleted] [BIT] NOT NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
)

GO
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT PK_Order
PRIMARY KEY (Id);

GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[OrderId] [BIGINT] NOT NULL,
	[ProductId] [BIGINT] NOT NULL,
	[OrderItemGuid] [UNIQUEIDENTIFIER] NOT NULL,
	[Quantity] [INT] NOT NULL,
	[OriginalProductCost] [DECIMAL](18, 4) NOT NULL,
	[ItemWeight] [DECIMAL](18, 4) NULL
);

GO
ALTER TABLE [dbo].[OrderItem]
ADD CONSTRAINT PK_OrderItem
PRIMARY KEY (Id);

GO
ALTER TABLE [dbo].[OrderItem]
ADD CONSTRAINT FK_OrderItem_Order
FOREIGN KEY (OrderId) REFERENCES [dbo].[Order](Id);

GO
ALTER TABLE [dbo].[OrderItem]
ADD CONSTRAINT FK_OrderItem_Product
FOREIGN KEY (ProductId) REFERENCES [dbo].[Product](Id);

-- PRODUCT ATTRIBUTE --
CREATE TABLE [dbo].[ProductAttribute]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[StatusId] INT NOT NULL,
	[CreatedById] BIGINT NOT NULL,
	[CreatedDate] DATETIME2(7) NOT NULL,
	[UpdatedById] BIGINT NOT NULL,
	[UpdatedDate] DATETIME2(7) NOT NULL,
)

GO
ALTER TABLE dbo.[ProductAttribute]
ADD CONSTRAINT PK_ProductAttribute
PRIMARY KEY (Id);

-- PRODUCT ATTRIBUTE RELATION --
CREATE TABLE [dbo].[ProductAttributeRelation](
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[ProductAttributeId] INT NOT NULL,
	[ProductId] BIGINT NOT NULL,
	[TextPrompt] NVARCHAR(MAX) NULL,
	[IsRequired] BIT NOT NULL,
	[AttributeControlTypeId] INT NOT NULL,
	[DisplayOrder] INT NOT NULL
)

GO
ALTER TABLE [dbo].[ProductAttributeRelation]
ADD CONSTRAINT PK_ProductAttributeRelation
PRIMARY KEY (Id)

GO
ALTER TABLE [dbo].[ProductAttributeRelation]
ADD CONSTRAINT FK_ProductAttributeRelation_Product
FOREIGN KEY (ProductId) REFERENCES dbo.[Product](Id)

GO
ALTER TABLE [dbo].[ProductAttributeRelation]
ADD CONSTRAINT FK_ProductAttributeRelation_ProductAttribute
FOREIGN KEY (ProductAttributeId) REFERENCES dbo.[ProductAttribute](Id)

-- Product Attribute Relation Value --
GO
CREATE TABLE [dbo].[ProductAttributeRelationValue](
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(400) NOT NULL,
	[ProductAttributeRelationId] BIGINT NOT NULL,
	[PriceAdjustment] DECIMAL(18, 4) NOT NULL,
	[PricePercentageAdjustment] DECIMAL(18, 4) NOT NULL,
	[Quantity] INT NOT NULL,
	[DisplayOrder] INT NOT NULL
)

GO
ALTER TABLE [dbo].[ProductAttributeRelationValue]
ADD CONSTRAINT PK_ProductAttributeRelationValue
PRIMARY KEY (Id)

GO
ALTER TABLE [dbo].[ProductAttributeRelationValue]
ADD CONSTRAINT FK_ProductAttributeRelationValue_ProductAttributeRelation
FOREIGN KEY (ProductAttributeRelationId) REFERENCES dbo.[ProductAttributeRelation](Id)
--PRODUCT PRICE--
CREATE TABLE [dbo].[ProductPrice](
	Id BIGINT NOT NULL IDENTITY(1,1),
	[ProductId] BIGINT NOT NULL,
	[Price] DECIMAL(18, 4) NOT NULL,
	[PricedDate] DATETIME2 NOT NULL,
	[IsCurrent] BIT NOT NULL,
	[IsDiscounted] BIT NOT NULL,
	IsPublished BIT NOT NULL
)

GO
ALTER TABLE dbo.[ProductPrice]
ADD CONSTRAINT PK_ProductPrice
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[ProductPrice]
ADD CONSTRAINT FK_ProductPrice_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

--FARM PRODUCT--
GO
CREATE TABLE dbo.FarmProduct(
	Id BIGINT NOT NULL IDENTITY(1,1),
	FarmId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NULL
)

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT PK_FarmProduct
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT FK_FarmProduct_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT FK_FarmProduct_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);

--PRODUCT GROUP--
GO
CREATE TABLE dbo.ProductCommunity(
	Id BIGINT NOT NULL IDENTITY(1,1),
	CommunityId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.ProductCommunity
ADD CONSTRAINT PK_ProductCommunity
PRIMARY KEY (CommunityId, ProductId);

GO
ALTER TABLE dbo.ProductCommunity
ADD CONSTRAINT FK_CommunityProduct_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.ProductCommunity
ADD CONSTRAINT FK_CommunityProduct_Community
FOREIGN KEY (CommunityId) REFERENCES dbo.[Community](Id);

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
	TypeId TINYINT NOT NULL,
)

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT PK_UserPhoto
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT FK_UserPhoto_UserPhotoType
FOREIGN KEY (TypeId) REFERENCES dbo.UserPhotoType(Id);

-- ArticleCategory --
GO
CREATE TABLE dbo.ArticleCategory
(
	Id INT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentId INT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.ArticleCategory
ADD CONSTRAINT PK_Category
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.ArticleCategory
ADD CONSTRAINT FK_ArticleCategory_ParentCategory
FOREIGN KEY (ParentId) REFERENCES dbo.ArticleCategory(Id);

-- ARTICLE --
GO
CREATE TABLE dbo.Article
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000),
	Content NVARCHAR(MAX) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ArticleCategoryId INT NOT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.Article
ADD CONSTRAINT PK_Article
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.Article
ADD CONSTRAINT FK_Article_ArticleCategory
FOREIGN KEY (ArticleCategoryId) REFERENCES dbo.ArticleCategory(Id);

-- TAG --
GO
CREATE TABLE dbo.Tag
(
	Id INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.Tag
ADD CONSTRAINT PK_Tag
PRIMARY KEY (Id);

-- ARTICLE TAG --
GO
CREATE TABLE dbo.ArticleTag
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	ArticleId BIGINT NOT NULL,
	TagId INT NOT NULL
)

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT FK_ArticleTag_Article
FOREIGN KEY (ArticleId) REFERENCES dbo.Article(Id);

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT FK_ArticleTag_Tag
FOREIGN KEY (TagId) REFERENCES dbo.Tag(Id);

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT PK_ArticleTag
PRIMARY KEY (Id);

--PICTURE--
GO
CREATE TABLE dbo.[Picture](
	[Id] BIGINT NOT NULL IDENTITY(1,1),
    [MimeType] NVARCHAR(40) NOT NULL,
    [Filename] NVARCHAR(300) NULL,
	[Title] NVARCHAR(MAX) NULL,
	[Alt] NVARCHAR(MAX) NULL,
	[BinaryData] VARBINARY(MAX) NULL,
	[RelativePath] NVARCHAR(MAX) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	StatusId INT NOT NULL
)

GO
ALTER TABLE dbo.[Picture]
ADD CONSTRAINT PK_Picture
PRIMARY KEY (Id);

--ARTICLE PICTURE--
GO
CREATE TABLE dbo.[ArticlePicture](
	Id BIGINT NOT NULL IDENTITY(1,1),
	[ArticleId] BIGINT NOT NULL,
    [PictureId] BIGINT NOT NULL,
	[PictureTypeId] INT NULL
)

GO
ALTER TABLE dbo.[ArticlePicture]
ADD CONSTRAINT FK_ArticlePicture_Article
FOREIGN KEY (ArticleId) REFERENCES dbo.Article(Id);

GO
ALTER TABLE dbo.[ArticlePicture]
ADD CONSTRAINT FK_ArticlePicture_Picture
FOREIGN KEY (PictureId) REFERENCES dbo.[Picture](Id);

GO
ALTER TABLE dbo.[ArticlePicture]
ADD CONSTRAINT PK_ArticlePicture
PRIMARY KEY (Id);

--FARM PICTURE--
GO
CREATE TABLE dbo.[FarmPicture](
	[Id] BIGINT NOT NULL IDENTITY(1,1),
	[FarmId] BIGINT NOT NULL,
    [PictureId] BIGINT NOT NULL,
	[PictureTypeId] INT NULL
)

GO
ALTER TABLE dbo.[FarmPicture]
ADD CONSTRAINT FK_FarmPicture_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);

GO
ALTER TABLE dbo.[FarmPicture]
ADD CONSTRAINT FK_FarmPicture_Picture
FOREIGN KEY (PictureId) REFERENCES dbo.[Picture](Id);

GO
ALTER TABLE dbo.[FarmPicture]
ADD CONSTRAINT PK_FarmPicture
PRIMARY KEY (Id);


--PRODUCT PICTURE--
GO
CREATE TABLE dbo.[ProductPicture](
	Id BIGINT NOT NULL IDENTITY(1,1),
	[ProductId] BIGINT NOT NULL,
    [PictureId] BIGINT NOT NULL,
	[PictureTypeId] INT NULL
)

GO
ALTER TABLE dbo.[ProductPicture]
ADD CONSTRAINT FK_ProductPicture_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.[ProductPicture]
ADD CONSTRAINT FK_ProductPicture_Picture
FOREIGN KEY (PictureId) REFERENCES dbo.[Picture](Id);

GO
ALTER TABLE dbo.[ProductPicture]
ADD CONSTRAINT PK_ProductPicture
PRIMARY KEY (Id);

-- USER SHORTCUT--
GO
CREATE TABLE [dbo].[Shortcut]
(
	Id INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	[Url] NVARCHAR(2000) NULL,
	[Icon] NVARCHAR(2000) NULL,
	[TypeId] INT NULL,
	[StatusId] INT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedById BIGINT NOT NULL,
	[DisplayOrder] INT NULL
)

GO
ALTER TABLE [dbo].[Shortcut]
ADD CONSTRAINT PK_Shortcut
PRIMARY KEY (Id);
