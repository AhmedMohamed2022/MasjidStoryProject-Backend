using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class NotificationI18nUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "MessageKey",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MessageVariables",
                table: "Notifications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageKey",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "MessageVariables",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Notifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
