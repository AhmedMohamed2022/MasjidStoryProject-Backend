using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class fixEventTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Masjids_MasjidId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Masjids_MasjidId",
                table: "Events",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Masjids_MasjidId",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Masjids_MasjidId",
                table: "Events",
                column: "MasjidId",
                principalTable: "Masjids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
