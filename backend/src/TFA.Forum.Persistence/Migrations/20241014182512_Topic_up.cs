using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.Forum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Topic_up : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Forum_ForumId",
                table: "Topic");

            migrationBuilder.AlterColumn<Guid>(
                name: "ForumId",
                table: "Topic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Forum_ForumId",
                table: "Topic",
                column: "ForumId",
                principalTable: "Forum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Forum_ForumId",
                table: "Topic");

            migrationBuilder.AlterColumn<Guid>(
                name: "ForumId",
                table: "Topic",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Forum_ForumId",
                table: "Topic",
                column: "ForumId",
                principalTable: "Forum",
                principalColumn: "Id");
        }
    }
}
