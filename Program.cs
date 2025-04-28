using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Data;
using DataAccess.Services;
using Microsoft.Extensions.Options;
using DataAccess.Services.Interfaces;
namespace MusicalCollectionWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();

            //connecting to DB
            builder.Services.AddDbContext<MusicalCollectionDbContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("DataAccess")));
            
            // Register services
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IUserCollectionService, UserCollectionService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<ITrackService, TrackService>();

            // add AutoMapper
            builder.Services.AddAutoMapper(typeof(DataAccess.MapperProfile));

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}