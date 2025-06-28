using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class FinalSolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Media_Stories_StoryId",
            //    table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTags_Stories_StoryId",
                table: "StoryTags");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTags_Tags_TagId",
                table: "StoryTags");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTags_Stories_StoryId",
                table: "StoryTags",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTags_Tags_TagId",
                table: "StoryTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTags_Stories_StoryId",
                table: "StoryTags");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryTags_Tags_TagId",
                table: "StoryTags");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Stories_StoryId",
                table: "Likes",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Masjids_MasjidId",
                table: "Media",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Stories_StoryId",
                table: "Media",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Masjids_MasjidId",
                table: "Stories",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTags_Stories_StoryId",
                table: "StoryTags",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryTags_Tags_TagId",
                table: "StoryTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
