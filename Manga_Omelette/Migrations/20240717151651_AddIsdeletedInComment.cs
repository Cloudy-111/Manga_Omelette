using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manga_Omelette.Migrations
{
    /// <inheritdoc />
    public partial class AddIsdeletedInComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Comment");
        }
    }
}
