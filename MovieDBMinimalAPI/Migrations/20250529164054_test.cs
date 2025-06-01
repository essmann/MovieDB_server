using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieDBMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "imdbID", "Actors", "Awards", "BoxOffice", "Country", "DVD", "Director", "Genre", "Language", "Metascore", "MyRating", "Plot", "Poster", "Production", "Rated", "Released", "Response", "Runtime", "Title", "Type", "Website", "Writer", "Year", "imdbRating", "imdbVotes" },
                values: new object[,]
                {
                    { "tt0068646", "Marlon Brando, Al Pacino, James Caan", "Won 3 Oscars. 31 wins & 30 nominations.", "$134,966,411", "USA", "09 Oct 2001", "Francis Ford Coppola", "Crime, Drama", "English, Italian, Latin", "100", "9.0", "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.", "https://m.media-amazon.com/images/M/MV5B..._V1_SX300.jpg", "Paramount Pictures", "R", "24 Mar 1972", "True", "175 min", "The Godfather", "movie", "N/A", "Mario Puzo, Francis Ford Coppola", "1972", "9.2", "1,987,149" },
                    { "tt0111161", "Tim Robbins, Morgan Freeman, Bob Gunton", "Nominated for 7 Oscars. 21 wins & 37 nominations.", "$28,767,189", "USA", "21 Dec 1999", "Frank Darabont", "Drama", "English", "80", "8.8", "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.", "https://m.media-amazon.com/images/M/MV5B..._V1_SX300.jpg", "Columbia Pictures", "R", "14 Oct 1994", "True", "142 min", "The Shawshank Redemption", "movie", "N/A", "Stephen King, Frank Darabont", "1994", "9.3", "2,840,490" },
                    { "tt1375666", "Leonardo DiCaprio, Joseph Gordon-Levitt, Elliot Page", "Won 4 Oscars. 159 wins & 220 nominations.", "$292,576,195", "USA, UK", "07 Dec 2010", "Christopher Nolan", "Action, Adventure, Sci-Fi", "English, Japanese, French", "74", "8.5", "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.", "https://m.media-amazon.com/images/M/MV5B..._V1_SX300.jpg", "Warner Bros. Pictures", "PG-13", "16 Jul 2010", "True", "148 min", "Inception", "movie", "N/A", "Christopher Nolan", "2010", "8.8", "2,525,487" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateRegisteredAt", "Email" },
                values: new object[,]
                {
                    { "user1-guid-abc", new DateOnly(2025, 4, 29), "alice@example.com" },
                    { "user2-guid-xyz", new DateOnly(2025, 5, 14), "bob@example.com" }
                });

            migrationBuilder.InsertData(
                table: "RatedMovies",
                columns: new[] { "RatingId", "MovieId", "Rating", "UserId" },
                values: new object[,]
                {
                    { "rating1-user1-movie1", "tt0111161", 9, "user1-guid-abc" },
                    { "rating2-user2-movie2", "tt1375666", 8, "user2-guid-xyz" },
                    { "rating3-user1-movie3", "tt0068646", 10, "user1-guid-abc" }
                });

            migrationBuilder.InsertData(
                table: "WatchlistMovies",
                columns: new[] { "WatchlistId", "MovieId", "UserId" },
                values: new object[,]
                {
                    { "watchlist1-user1-movie2", "tt1375666", "user1-guid-abc" },
                    { "watchlist2-user2-movie1", "tt0111161", "user2-guid-xyz" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RatedMovies",
                keyColumn: "RatingId",
                keyValue: "rating1-user1-movie1");

            migrationBuilder.DeleteData(
                table: "RatedMovies",
                keyColumn: "RatingId",
                keyValue: "rating2-user2-movie2");

            migrationBuilder.DeleteData(
                table: "RatedMovies",
                keyColumn: "RatingId",
                keyValue: "rating3-user1-movie3");

            migrationBuilder.DeleteData(
                table: "WatchlistMovies",
                keyColumn: "WatchlistId",
                keyValue: "watchlist1-user1-movie2");

            migrationBuilder.DeleteData(
                table: "WatchlistMovies",
                keyColumn: "WatchlistId",
                keyValue: "watchlist2-user2-movie1");

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "imdbID",
                keyValue: "tt0068646");

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "imdbID",
                keyValue: "tt0111161");

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "imdbID",
                keyValue: "tt1375666");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "user1-guid-abc");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "user2-guid-xyz");
        }
    }
}
