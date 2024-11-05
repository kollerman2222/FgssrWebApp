using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fgssr.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateprivatemeesagetableremovenavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_ApplicationUserId",
                table: "PrivateMessage");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessage_ApplicationUserId",
                table: "PrivateMessage");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PrivateMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "PrivateMessage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessage_ApplicationUserId",
                table: "PrivateMessage",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_ApplicationUserId",
                table: "PrivateMessage",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
