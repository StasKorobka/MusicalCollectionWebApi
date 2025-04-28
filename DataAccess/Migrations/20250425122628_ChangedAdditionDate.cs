using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAdditionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisbandDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfTracks = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<TimeSpan>(type: "time", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<TimeSpan>(type: "time", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    PositionInAlbum = table.Column<int>(type: "int", nullable: false),
                    Songwriters = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.TrackId);
                    table.ForeignKey(
                        name: "FK_Tracks_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCollections",
                columns: table => new
                {
                    UserCollectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    AdditionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollections", x => x.UserCollectionId);
                    table.ForeignKey(
                        name: "FK_UserCollections_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTracks",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTracks", x => new { x.PlaylistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "ArtistId", "Biography", "Country", "DisbandDate", "FormationDate", "Genres", "Name" },
                values: new object[,]
                {
                    { 1, "The Beatles were an English rock band formed in Liverpool in 1960.", "United Kingdom", new DateTime(1970, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rock, Pop", "The Beatles" },
                    { 2, "AC/DC is a cool rocking band", "Australia", null, new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rock, Rock'n'Roll", "AC/DC" },
                    { 3, "Linkin Park was an American rock band known for blending nu metal, alternative rock, and rap.", "USA", new DateTime(2017, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rock, Nu Metal, Alternative", "Linkin Park" },
                    { 4, "Taylor Swift is an American singer-songwriter known for narrative songs and genre-crossing albums.", "USA", null, new DateTime(2006, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pop, Country, Folk", "Taylor Swift" },
                    { 5, "Imagine Dragons is an American pop rock band from Las Vegas known for energetic hits and anthemic sound.", "USA", null, new DateTime(2008, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pop Rock, Electropop", "Imagine Dragons" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreationDate", "FirstName", "LastName" },
                values: new object[] { 1, new DateTime(2025, 4, 25, 12, 26, 27, 984, DateTimeKind.Utc).AddTicks(4957), "John", "Doe" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "AlbumId", "ArtistId", "Format", "Genres", "Label", "Length", "NumberOfTracks", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "Rock,Pop", "Apple Records", new TimeSpan(0, 0, 47, 3, 0), 17, new DateOnly(1969, 9, 26), "Abbey Road" },
                    { 2, 2, 0, "Rock,Glam", "Republic Records", new TimeSpan(0, 0, 41, 55, 0), 13, new DateOnly(1982, 9, 26), "T.N.T" },
                    { 3, 3, 0, "Nu Metal,Alternative", "Warner Bros.", new TimeSpan(0, 0, 37, 52, 0), 12, new DateOnly(2000, 10, 24), "Hybrid Theory" },
                    { 4, 4, 0, "Pop,Synth-pop", "Republic", new TimeSpan(0, 1, 17, 49, 0), 13, new DateOnly(2014, 10, 27), "1989" },
                    { 5, 5, 2, "Pop Rock,Alternative", "KIDinaKORNER, Interscope", new TimeSpan(0, 0, 39, 12, 0), 11, new DateOnly(2017, 6, 23), "Evolve" },
                    { 6, 3, 0, "Nu Metal,Alternative", "Warner Bros.", new TimeSpan(0, 0, 36, 43, 0), 13, new DateOnly(2003, 3, 25), "Meteora" }
                });

            migrationBuilder.InsertData(
                table: "Playlists",
                columns: new[] { "PlaylistId", "CreationDate", "PlaylistName", "UserId" },
                values: new object[] { 1, new DateTime(2025, 4, 25, 12, 26, 27, 984, DateTimeKind.Utc).AddTicks(8535), "Favorites", 1 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "AlbumId", "Comment", "Rating", "UserId" },
                values: new object[] { 1, 1, "One of the best albums ever!", 5, 1 });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "TrackId", "AlbumId", "Length", "PositionInAlbum", "Songwriters", "Title" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 0, 4, 19, 0), 1, "John Lennon, Paul McCartney", "Come Together" },
                    { 2, 1, new TimeSpan(0, 0, 3, 2, 0), 2, "George Harrison", "Something" },
                    { 3, 2, new TimeSpan(0, 0, 2, 5, 0), 5, "Angus Young, Malcolm Young, Bon Scott", "Thundertruck" },
                    { 4, 2, new TimeSpan(0, 0, 3, 5, 0), 6, "George Harrison", "Hells Bells" },
                    { 5, 3, new TimeSpan(0, 0, 3, 36, 0), 8, "Linkin Park", "In the End" },
                    { 6, 4, new TimeSpan(0, 0, 3, 51, 0), 2, "Taylor Swift, Max Martin, Shellback", "Blank Space" },
                    { 7, 5, new TimeSpan(0, 0, 3, 24, 0), 1, "Imagine Dragons", "Believer" },
                    { 8, 6, new TimeSpan(0, 0, 3, 7, 0), 13, "Linkin Park", "Numb" }
                });

            migrationBuilder.InsertData(
                table: "UserCollections",
                columns: new[] { "UserCollectionId", "AdditionDate", "AlbumId", "Status", "UserId" },
                values: new object[] { 1, new DateOnly(2025, 4, 25), 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "PlaylistTracks",
                columns: new[] { "PlaylistId", "TrackId", "Position" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AlbumId",
                table: "Reviews",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumId",
                table: "Tracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollections_AlbumId",
                table: "UserCollections",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollections_UserId",
                table: "UserCollections",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistTracks");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "UserCollections");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
