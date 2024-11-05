using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fgssr.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_models_first_version : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiplomasDepartments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiplomasDepartments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    EventLocation = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    DateDay = table.Column<int>(type: "int", nullable: false),
                    DateMonth = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EventImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NewsDescription = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    NewsDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Biograpghy = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "DiplomasSections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    SectionImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiplomasSections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_DiplomasSections_DiplomasDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DiplomasDepartments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiplomasSections_DepartmentId",
                table: "DiplomasSections",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiplomasSections");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "DiplomasDepartments");
        }
    }
}
