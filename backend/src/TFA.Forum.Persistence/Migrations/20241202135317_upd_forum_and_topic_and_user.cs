using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.Forum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class upd_forum_and_topic_and_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Topic",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Topic",
                newName: "content");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Topic",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Topic",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Topic",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Topic",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
