using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoryAndMediaEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MasjidId",
                table: "Media",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_StoryId",
                table: "Media",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_StoryId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Media");

            migrationBuilder.AlterColumn<int>(
                name: "MasjidId",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
