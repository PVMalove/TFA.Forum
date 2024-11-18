using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFA.Forum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class up_forum_add_time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Topic",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Comment",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Topic",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Forum",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Forum",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Comment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Forum");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Forum");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Topic",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Comment",
                newName: "CreateAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateAt",
                table: "Topic",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateAt",
                table: "Comment",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
