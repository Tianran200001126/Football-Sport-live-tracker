using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsEventsTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckConstraintToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Match_TeamNamesNotEqual",
                table: "Matches",
                sql: "[TeamAName] != [TeamBName]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Match_TeamNamesNotEqual",
                table: "Matches");
        }
    }
}
