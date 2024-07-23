using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manga_Omelette.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteCommentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment",
                column: "ParentCommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment",
                column: "ParentCommentId",
                principalTable: "Comment",
                principalColumn: "Id");
        }
    }
}
