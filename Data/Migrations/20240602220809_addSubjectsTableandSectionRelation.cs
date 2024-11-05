using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fgssr.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSubjectsTableandSectionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectNameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectNameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ScientificDegree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                    table.ForeignKey(
                        name: "FK_Subjects_DiplomasSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "DiplomasSections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SectionId",
                table: "Subjects",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
