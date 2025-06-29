using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaCascadeDeleteAndProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Media",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Media",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Media",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Media",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "MasjidId1",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoryId1",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_DateUploaded",
                table: "Media",
                column: "DateUploaded");

            migrationBuilder.CreateIndex(
                name: "IX_Media_MasjidId1",
                table: "Media",
                column: "MasjidId1");

            migrationBuilder.CreateIndex(
                name: "IX_Media_StoryId1",
                table: "Media",
                column: "StoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Masjids_MasjidId1",
                table: "Media",
                column: "MasjidId1",
                principalTable: "Masjids",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_StoryId1",
                table: "Media",
                column: "StoryId1",
                principalTable: "Stories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Masjids_MasjidId1",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_StoryId1",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Media_DateUploaded",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_MasjidId1",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_StoryId1",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "MasjidId1",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "StoryId1",
                table: "Media");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id");
        }
    }
}
