using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbiWebsite.Migrations
{
    /// <inheritdoc />
    public partial class MailAndLoginCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Traits",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LoginCode",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LoginCode",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Traits",
                table: "Students",
                type: "TEXT",
                nullable: true);
        }
    }
}
