using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.Forum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class up_forum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Forum",
                newName: "title");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Forum",
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
                table: "Forum",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Forum",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
