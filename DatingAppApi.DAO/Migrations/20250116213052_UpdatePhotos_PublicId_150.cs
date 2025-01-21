using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingAppApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotos_PublicId_150 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UsersId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            //migrationBuilder.RenameTable(
            //    name: "Users",
            //    newName: "Users");

            //migrationBuilder.RenameColumn(
            //    name: "AppUsersId",
            //    table: "Photos",
            //    newName: "UsersId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Photos_AppUsersId",
            //    table: "Photos",
            //    newName: "IX_Photos_UsersId");

            migrationBuilder.AlterColumn<string>(
                name: "PublicId",
                table: "Photos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_UsersId",
                table: "Photos",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UsersId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Photos",
                newName: "AppUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UsersId",
                table: "Photos",
                newName: "IX_Photos_AppUsersId");

            migrationBuilder.AlterColumn<string>(
                name: "PublicId",
                table: "Photos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AppUsers_AppUsersId",
                table: "Photos",
                column: "AppUsersId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
