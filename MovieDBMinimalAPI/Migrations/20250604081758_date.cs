using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDBMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "AddeddAt",
                table: "WatchlistMovies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "RatedAt",
                table: "RatedMovies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddeddAt",
                table: "WatchlistMovies");

            migrationBuilder.DropColumn(
                name: "RatedAt",
                table: "RatedMovies");
        }
    }
}
