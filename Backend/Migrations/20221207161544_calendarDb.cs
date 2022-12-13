using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class calendarDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "EventCategories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descr = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DOB = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OrganizationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VenueCategories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descr = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VenueName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Coordinates = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    WheelchairAccess = table.Column<bool>(type: "boolean", nullable: false),
                    Food = table.Column<bool>(type: "boolean", nullable: false),
                    Drink = table.Column<bool>(type: "boolean", nullable: false),
                    AverageRating = table.Column<decimal>(type: "numeric", nullable: false),
                    VenueCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venues_VenueCategories_VenueCategoryId",
                        column: x => x.VenueCategoryId,
                        principalSchema: "public",
                        principalTable: "VenueCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descr = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AgeRestricted = table.Column<bool>(type: "boolean", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "integer", nullable: false),
                    CurrentStatus = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<decimal>(type: "numeric", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalSchema: "public",
                        principalTable: "EventCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueId",
                        column: x => x.VenueId,
                        principalSchema: "public",
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueMedias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Media = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueMedias_Venues_VenueId",
                        column: x => x.VenueId,
                        principalSchema: "public",
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueReviews",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueReviews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueReviews_Venues_VenueId",
                        column: x => x.VenueId,
                        principalSchema: "public",
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendings_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "public",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventMedias",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Media = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventMedias_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "public",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventReviews",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventReviews_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "public",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventReviews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "EventCategories",
                columns: new[] { "Id", "CategoryName", "Descr" },
                values: new object[,]
                {
                    { 1, "Concert", "Live music" },
                    { 2, "Play", "Theatrical presentation" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Users",
                columns: new[] { "Id", "ContactName", "DOB", "Email", "IsBanned", "OrganizationName", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "", new DateTimeOffset(new DateTime(2004, 12, 7, 16, 15, 44, 660, DateTimeKind.Unspecified).AddTicks(9452), new TimeSpan(0, 0, 0, 0, 0)), "User@Test.com", false, "", "$2a$11$S2NuC3VfU82Ks2FCVcCNR.bHC44kAPk24fWa7JemBgvsdECG/DcC2", 0, "TestUser" },
                    { 2, "", new DateTimeOffset(new DateTime(2004, 12, 7, 16, 15, 44, 793, DateTimeKind.Unspecified).AddTicks(5337), new TimeSpan(0, 0, 0, 0, 0)), "Admin@Test.com", false, "", "$2a$11$Mk9i9ONCVGdqwAKo0rBDFeblTBPhOh1eFd0WhrtNHtK6qzNOSVWB6", 1, "TestAdmin" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "VenueCategories",
                columns: new[] { "Id", "CategoryName", "Descr" },
                values: new object[,]
                {
                    { 1, "Bar", "Drinking establishment" },
                    { 2, "Theatre", "Area for dramatic performances" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendings_EventId",
                schema: "public",
                table: "Attendings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendings_UserId",
                schema: "public",
                table: "Attendings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventMedias_EventId",
                schema: "public",
                table: "EventMedias",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventReviews_EventId",
                schema: "public",
                table: "EventReviews",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventReviews_UserId",
                schema: "public",
                table: "EventReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategoryId",
                schema: "public",
                table: "Events",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                schema: "public",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                schema: "public",
                table: "Events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueMedias_VenueId",
                schema: "public",
                table: "VenueMedias",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueReviews_UserId",
                schema: "public",
                table: "VenueReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueReviews_VenueId",
                schema: "public",
                table: "VenueReviews",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Venues_VenueCategoryId",
                schema: "public",
                table: "Venues",
                column: "VenueCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EventMedias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EventReviews",
                schema: "public");

            migrationBuilder.DropTable(
                name: "VenueMedias",
                schema: "public");

            migrationBuilder.DropTable(
                name: "VenueReviews",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EventCategories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Venues",
                schema: "public");

            migrationBuilder.DropTable(
                name: "VenueCategories",
                schema: "public");
        }
    }
}
