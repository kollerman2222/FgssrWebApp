using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fgssr.Data.Migrations
{
    /// <inheritdoc />
    public partial class addprofileimagetochatmessagemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "ChatMessages");
        }
    }
}
