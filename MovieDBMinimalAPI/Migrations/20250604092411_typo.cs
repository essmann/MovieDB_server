using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDBMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddeddAt",
                table: "WatchlistMovies",
                newName: "AddedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddedAt",
                table: "WatchlistMovies",
                newName: "AddeddAt");
        }
    }
}
