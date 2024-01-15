using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterCloneAPI.Migrations
{
    /// <inheritdoc />
    public partial class added_timestamps_in_comment_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Comment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Comment",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Comment");
        }
    }
}
