using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
