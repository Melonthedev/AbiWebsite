using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbiWebsite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_MottoSuggestions_MottoSuggestionId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Students_StudentId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_StudentId",
                table: "Vote");

            migrationBuilder.RenameTable(
                name: "Vote",
                newName: "Votes");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_MottoSuggestionId",
                table: "Votes",
                newName: "IX_Votes_MottoSuggestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_StudentId_MottoSuggestionId",
                table: "Votes",
                columns: new[] { "StudentId", "MottoSuggestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_MottoSuggestions_MottoSuggestionId",
                table: "Votes",
                column: "MottoSuggestionId",
                principalTable: "MottoSuggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Students_StudentId",
                table: "Votes",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_MottoSuggestions_MottoSuggestionId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Students_StudentId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_StudentId_MottoSuggestionId",
                table: "Votes");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "Vote");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_MottoSuggestionId",
                table: "Vote",
                newName: "IX_Vote_MottoSuggestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_StudentId",
                table: "Vote",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_MottoSuggestions_MottoSuggestionId",
                table: "Vote",
                column: "MottoSuggestionId",
                principalTable: "MottoSuggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Students_StudentId",
                table: "Vote",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
