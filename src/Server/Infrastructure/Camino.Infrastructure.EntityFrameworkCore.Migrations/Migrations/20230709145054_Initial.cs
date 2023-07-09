using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camino.Infrastructure.EntityFrameworkCore.Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "ArticleCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleCategory_ArticleCategory_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "ArticleCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomOrderNumber = table.Column<string>(type: "text", nullable: false),
                    BillingAddress = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    PickupAddress = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress = table.Column<string>(type: "text", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    IsPickupInStore = table.Column<bool>(type: "boolean", nullable: false),
                    OrderStatusId = table.Column<int>(type: "integer", nullable: false),
                    ShippingStatusId = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    OrderDiscount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OrderTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CustomerIp = table.Column<string>(type: "text", nullable: false),
                    PaidDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippingMethod = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Alt = table.Column<string>(type: "text", nullable: true),
                    BinaryData = table.Column<byte[]>(type: "bytea", nullable: true),
                    RelativePath = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_ProductCategory_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "ProductCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shortcut",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shortcut", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAttribute",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPhotoType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhotoType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Content = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ArticleCategoryId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_ArticleCategory_ArticleCategoryId",
                        column: x => x.ArticleCategoryId,
                        principalSchema: "dbo",
                        principalTable: "ArticleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farm",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    FarmTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farm_FarmType_FarmTypeId",
                        column: x => x.FarmTypeId,
                        principalSchema: "dbo",
                        principalTable: "FarmType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    OrderItemGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OriginalProductCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ItemWeight = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPicture",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    PictureId = table.Column<long>(type: "bigint", nullable: false),
                    PictureTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPicture_Picture_PictureId",
                        column: x => x.PictureId,
                        principalSchema: "dbo",
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPicture_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    IsDiscounted = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PricedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeRelation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductAttributeId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    TextPrompt = table.Column<string>(type: "text", nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    AttributeControlTypeId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeRelation_ProductAttribute_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalSchema: "dbo",
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryRelation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryRelation_Product_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryRelation_ProductCategory_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalSchema: "dbo",
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Lastname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Firstname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GenderId = table.Column<int>(type: "integer", nullable: true),
                    CountryId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "auth",
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Gender_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "auth",
                        principalTable: "Gender",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "auth",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_User_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPhoto",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPhoto_UserPhotoType_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "dbo",
                        principalTable: "UserPhotoType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticlePicture",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false),
                    PictureId = table.Column<long>(type: "bigint", nullable: false),
                    PictureTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticlePicture_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "dbo",
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticlePicture_Picture_PictureId",
                        column: x => x.PictureId,
                        principalSchema: "dbo",
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmPicture",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FarmId = table.Column<long>(type: "bigint", nullable: false),
                    PictureId = table.Column<long>(type: "bigint", nullable: false),
                    PictureTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmPicture_Farm_FarmId",
                        column: x => x.FarmId,
                        principalSchema: "dbo",
                        principalTable: "Farm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmPicture_Picture_PictureId",
                        column: x => x.PictureId,
                        principalSchema: "dbo",
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmProduct",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FarmId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    LinkedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsLinked = table.Column<bool>(type: "boolean", nullable: false),
                    LinkedById = table.Column<long>(type: "bigint", nullable: false),
                    ApprovedById = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmProduct_Farm_FarmId",
                        column: x => x.FarmId,
                        principalSchema: "dbo",
                        principalTable: "Farm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeRelationValue",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductAttributeRelationId = table.Column<long>(type: "bigint", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PricePercentageAdjustment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeRelationValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeRelationValue_ProductAttributeRelation_Prod~",
                        column: x => x.ProductAttributeRelationId,
                        principalSchema: "dbo",
                        principalTable: "ProductAttributeRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationPolicy",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationPolicy_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthorizationPolicy_User_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_User_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: false),
                    ClaimValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizationPolicy",
                schema: "auth",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizationPolicyId = table.Column<long>(type: "bigint", nullable: false),
                    GrantedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedById = table.Column<long>(type: "bigint", nullable: false),
                    IsGranted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizationPolicy", x => new { x.UserId, x.AuthorizationPolicyId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizationPolicy_AuthorizationPolicy_AuthorizationPo~",
                        column: x => x.AuthorizationPolicyId,
                        principalSchema: "auth",
                        principalTable: "AuthorizationPolicy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAuthorizationPolicy_User_GrantedById",
                        column: x => x.GrantedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAuthorizationPolicy_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleAuthorizationPolicy",
                schema: "auth",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizationPolicyId = table.Column<long>(type: "bigint", nullable: false),
                    GrantedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedById = table.Column<long>(type: "bigint", nullable: false),
                    IsGranted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAuthorizationPolicy", x => new { x.RoleId, x.AuthorizationPolicyId });
                    table.ForeignKey(
                        name: "FK_RoleAuthorizationPolicy_AuthorizationPolicy_AuthorizationPo~",
                        column: x => x.AuthorizationPolicyId,
                        principalSchema: "auth",
                        principalTable: "AuthorizationPolicy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleAuthorizationPolicy_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleAuthorizationPolicy_User_GrantedById",
                        column: x => x.GrantedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: false),
                    ClaimValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "auth",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    GrantedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedById = table.Column<long>(type: "bigint", nullable: false),
                    IsGranted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRole_User_GrantedById",
                        column: x => x.GrantedById,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_ArticleCategoryId",
                schema: "dbo",
                table: "Article",
                column: "ArticleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategory_ParentId",
                schema: "dbo",
                table: "ArticleCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePicture_ArticleId",
                schema: "dbo",
                table: "ArticlePicture",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePicture_PictureId",
                schema: "dbo",
                table: "ArticlePicture",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationPolicy_CreatedById",
                schema: "auth",
                table: "AuthorizationPolicy",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationPolicy_UpdatedById",
                schema: "auth",
                table: "AuthorizationPolicy",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Farm_FarmTypeId",
                schema: "dbo",
                table: "Farm",
                column: "FarmTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmPicture_FarmId",
                schema: "dbo",
                table: "FarmPicture",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmPicture_PictureId",
                schema: "dbo",
                table: "FarmPicture",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmProduct_FarmId",
                schema: "dbo",
                table: "FarmProduct",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmProduct_ProductId",
                schema: "dbo",
                table: "FarmProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "dbo",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeRelation_ProductAttributeId",
                schema: "dbo",
                table: "ProductAttributeRelation",
                column: "ProductAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeRelationValue_ProductAttributeRelationId",
                schema: "dbo",
                table: "ProductAttributeRelationValue",
                column: "ProductAttributeRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ParentId",
                schema: "dbo",
                table: "ProductCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryRelation_ProductCategoryId",
                schema: "dbo",
                table: "ProductCategoryRelation",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPicture_PictureId",
                schema: "dbo",
                table: "ProductPicture",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPicture_ProductId",
                schema: "dbo",
                table: "ProductPicture",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_ProductId",
                schema: "dbo",
                table: "ProductPrice",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedById",
                schema: "auth",
                table: "Role",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Role_UpdatedById",
                schema: "auth",
                table: "Role",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAuthorizationPolicy_AuthorizationPolicyId",
                schema: "auth",
                table: "RoleAuthorizationPolicy",
                column: "AuthorizationPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAuthorizationPolicy_GrantedById",
                schema: "auth",
                table: "RoleAuthorizationPolicy",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                schema: "auth",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CountryId",
                schema: "auth",
                table: "User",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedById",
                schema: "auth",
                table: "User",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_User_GenderId",
                schema: "auth",
                table: "User",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_StatusId",
                schema: "auth",
                table: "User",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedById",
                schema: "auth",
                table: "User",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationPolicy_AuthorizationPolicyId",
                schema: "auth",
                table: "UserAuthorizationPolicy",
                column: "AuthorizationPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationPolicy_GrantedById",
                schema: "auth",
                table: "UserAuthorizationPolicy",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                schema: "auth",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                schema: "auth",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhoto_TypeId",
                schema: "dbo",
                table: "UserPhoto",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_GrantedById",
                schema: "auth",
                table: "UserRole",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "auth",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                schema: "auth",
                table: "UserToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticlePicture",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FarmPicture",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FarmProduct",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductAttributeRelationValue",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductCategoryRelation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductPicture",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductPrice",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RoleAuthorizationPolicy",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "RoleClaim",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Shortcut",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserAttribute",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserAuthorizationPolicy",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserClaim",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserPhoto",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Article",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Farm",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductAttributeRelation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Picture",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuthorizationPolicy",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserPhotoType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "ArticleCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FarmType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductAttribute",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Gender",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "auth");
        }
    }
}
