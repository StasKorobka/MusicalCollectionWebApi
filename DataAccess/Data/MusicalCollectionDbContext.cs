using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DataAccess.Models;
using DataAccess.Models.Enums;

namespace DataAccess.Data
{
    public class MusicalCollectionDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public MusicalCollectionDbContext(DbContextOptions<MusicalCollectionDbContext> options)
            :base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connecting to DB
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var stringListConverter = new ValueConverter<List<string>, string>(
               v => string.Join(',', v),
               v => v.Split(',', StringSplitOptions.None).ToList());

            var stringListComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            // configurations, relations and restrictins
            // Album
            // For a single entity:
            
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting artist deletes album
           modelBuilder.Entity<Album>()
                .Property(a => a.Genres)
                .HasConversion(stringListConverter)
                .Metadata.SetValueComparer(stringListComparer);//set str comparer

            // Track
            modelBuilder.Entity<Track>()
                .HasOne(t => t.Album)
                .WithMany(a => a.Tracks)
                .HasForeignKey(t => t.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserCollection
            modelBuilder.Entity<UserCollection>()
                .Property(e => e.UserCollectionId)
                .UseIdentityColumn(seed: 1, increment: 1);
            modelBuilder.Entity<UserCollection>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserCollection>()
                .HasOne(uc => uc.Album)
                .WithMany(a => a.UserCollections)
                .HasForeignKey(uc => uc.AlbumId);

            // Review
            modelBuilder.Entity<Review>()
                .Property(e => e.ReviewId)
                .UseIdentityColumn(seed: 1, increment: 1);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Album)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AlbumId);

            // Playlist
            modelBuilder.Entity<Playlist>()
                .Property(e => e.PlaylistId)
                .UseIdentityColumn(seed: 1, increment: 1);
            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId);

            // PlaylistTrack (many-tо-many between Playlist and Track)
            modelBuilder.Entity<PlaylistTrack>()
                .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.PlaylistId);

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Track)
                .WithMany(t => t.PlaylistTracks)
                .HasForeignKey(pt => pt.TrackId);

            // Initial data (Seeding)
            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {

            // Adding Artist
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    ArtistId = 1,
                    Name = "The Beatles",
                    Country = "United Kingdom",
                    FormationDate = new DateTime(1960, 1, 1),
                    DisbandDate = new DateTime(1970, 4, 10),
                    Genres = "Rock, Pop",
                    Biography = "The Beatles were an English rock band formed in Liverpool in 1960."
                },
                new Artist
                {
                    ArtistId = 2,
                    Name = "AC/DC",
                    Country = "Australia",
                    FormationDate = new DateTime(1960, 1, 1),
                    DisbandDate = null,//new DateTime(2025, 4, 10),
                    Genres = "Rock, Rock'n'Roll",
                    Biography = "AC/DC is a cool rocking band"
                },
                new Artist
                {
                    ArtistId = 3,
                    Name = "Linkin Park",
                    Country = "USA",
                    FormationDate = new DateTime(1996, 1, 1),
                    DisbandDate = new DateTime(2017, 7, 20),
                    Genres = "Rock, Nu Metal, Alternative",
                    Biography = "Linkin Park was an American rock band known for blending nu metal, alternative rock, and rap."
                },
                new Artist
                {
                    ArtistId = 4,
                    Name = "Taylor Swift",
                    Country = "USA",
                    FormationDate = new DateTime(2006, 10, 24),
                    DisbandDate = null,
                    Genres = "Pop, Country, Folk",
                    Biography = "Taylor Swift is an American singer-songwriter known for narrative songs and genre-crossing albums."
                },
                new Artist
                {
                    ArtistId = 5,
                    Name = "Imagine Dragons",
                    Country = "USA",
                    FormationDate = new DateTime(2008, 1, 1),
                    DisbandDate = null,
                    Genres = "Pop Rock, Electropop",
                    Biography = "Imagine Dragons is an American pop rock band from Las Vegas known for energetic hits and anthemic sound."
                }
            );

            // Adding album
            modelBuilder.Entity<Album>().HasData(
                new Album
                {
                    AlbumId = 1,
                    Title = "Abbey Road",
                    ArtistId = 1,
                    Genres = new List<string> { "Rock", "Pop" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(1969, 9, 26)),
                    NumberOfTracks = 17,
                    Label = "Apple Records",
                    Format = AlbumFormat.Vinyl,
                    Length = TimeSpan.FromMinutes(47).Add(TimeSpan.FromSeconds(3)),
                },
                new Album
                {
                    AlbumId = 2,
                    Title = "T.N.T",
                    ArtistId = 2,
                    Genres = new List<string> { "Rock", "Glam" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(1982, 9, 26)),
                    NumberOfTracks = 13,
                    Label = "Republic Records",
                    Format = AlbumFormat.CD,
                    Length = TimeSpan.FromMinutes(41).Add(TimeSpan.FromSeconds(55))
                },
                new Album
                {
                    AlbumId = 3,
                    Title = "Hybrid Theory",
                    ArtistId = 3,
                    Genres = new List<string> { "Nu Metal", "Alternative" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2000, 10, 24)),
                    NumberOfTracks = 12,
                    Label = "Warner Bros.",
                    Format = AlbumFormat.CD,
                    Length = TimeSpan.FromMinutes(37).Add(TimeSpan.FromSeconds(52))
                },
                new Album
                {
                    AlbumId = 4,
                    Title = "1989",
                    ArtistId = 4,
                    Genres = new List<string> { "Pop", "Synth-pop" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2014, 10, 27)),
                    NumberOfTracks = 13,
                    Label = "Republic",
                    Format = AlbumFormat.CD,
                    Length = TimeSpan.FromMinutes(77).Add(TimeSpan.FromSeconds(49))
                },
                new Album
                {
                    AlbumId = 5,
                    Title = "Evolve",
                    ArtistId = 5,
                    Genres = new List<string> { "Pop Rock", "Alternative" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2017, 6, 23)),
                    NumberOfTracks = 11,
                    Label = "KIDinaKORNER, Interscope",
                    Format = AlbumFormat.Digital,
                    Length = TimeSpan.FromMinutes(39).Add(TimeSpan.FromSeconds(12))
                },
                new Album
                {
                    AlbumId = 6,
                    Title = "Meteora",
                    ArtistId = 3,
                    Genres = new List<string> { "Nu Metal", "Alternative" },
                    ReleaseDate = DateOnly.FromDateTime(new DateTime(2003, 3, 25)),
                    NumberOfTracks = 13,
                    Label = "Warner Bros.",
                    Format = AlbumFormat.CD,
                    Length = TimeSpan.FromMinutes(36).Add(TimeSpan.FromSeconds(43))
                }
            );

            // Adding tracks
            modelBuilder.Entity<Track>().HasData(
                new Track
                {
                    TrackId = 1,
                    Title = "Come Together",
                    Length = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(19)),
                    AlbumId = 1,
                    PositionInAlbum = 1,
                    Songwriters = "John Lennon, Paul McCartney"
                },
                new Track
                {
                    TrackId = 2,
                    Title = "Something",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(2)),
                    AlbumId = 1,
                    PositionInAlbum = 2,
                    Songwriters = "George Harrison"
                },
                new Track
                {
                     TrackId = 3,
                     Title = "Thundertruck",
                     Length = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(5)),
                     AlbumId = 2,
                     PositionInAlbum = 5,
                     Songwriters = "Angus Young, Malcolm Young, Bon Scott"
                },
                new Track
                {
                    TrackId = 4,
                    Title = "Hells Bells",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(5)),
                    AlbumId = 2,
                    PositionInAlbum = 6,
                    Songwriters = "George Harrison"
                },
                new Track
                {
                    TrackId = 5,
                    Title = "In the End",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(36)),
                    AlbumId = 3,
                    PositionInAlbum = 8,
                    Songwriters = "Linkin Park"
                },
                new Track
                {
                    TrackId = 6,
                    Title = "Blank Space",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(51)),
                    AlbumId = 4,
                    PositionInAlbum = 2,
                    Songwriters = "Taylor Swift, Max Martin, Shellback"
                },
                new Track
                {
                    TrackId = 7,
                    Title = "Believer",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)),
                    AlbumId = 5,
                    PositionInAlbum = 1,
                    Songwriters = "Imagine Dragons"
                },
                new Track
                {
                    TrackId = 8,
                    Title = "Numb",
                    Length = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(7)),
                    AlbumId = 6,
                    PositionInAlbum = 13,
                    Songwriters = "Linkin Park"
                }
            );

            // Adding user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    CreationDate = DateTime.UtcNow
                }
            );

            // Adding user collection
            modelBuilder.Entity<UserCollection>().HasData(
                new UserCollection
                {
                    UserCollectionId = 1,
                    UserId = 1,
                    AlbumId = 1,
                    AdditionDate = DateTime.UtcNow,
                    Status = CollectionStatus.Wishlist
                }
            );

            // Adding review
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewId = 1,
                    UserId = 1,
                    AlbumId = 1,
                    Rating = StarRating.FiveStars,
                    Comment = "One of the best albums ever!"
                }
            );

            // Adding playlist
            modelBuilder.Entity<Playlist>().HasData(
                new Playlist
                {
                    PlaylistId = 1,
                    PlaylistName = "Favorites",
                    UserId = 1,
                    CreationDate = DateTime.UtcNow
                }
            );

            // Adding tracks to playlist
            modelBuilder.Entity<PlaylistTrack>().HasData(
                new PlaylistTrack { PlaylistId = 1, TrackId = 1, Position = 1 }
            );
        }
    }
}