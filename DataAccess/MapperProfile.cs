using AutoMapper;
using DataAccess.Models;
using DataAccess.Models.DTO.Album;
using DataAccess.Models.DTO.Artist;
using DataAccess.Models.DTO.Playlist;
using DataAccess.Models.DTO.PlaylistTrack;
using DataAccess.Models.DTO.Review;
using DataAccess.Models.DTO.Track;
using DataAccess.Models.DTO.UserCollection;

namespace DataAccess
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Track, TrackDto>();
            CreateMap<Artist, ArtistDto>();
            CreateMap<Album, AlbumDto>();
            CreateMap<UserCollection, UserCollectionDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Playlist, PlaylistDto>();

            //playlist to ShowPlaylistDto
            CreateMap<Playlist, ShowPlaylistDto>()
                .ForMember(dest => dest.Tracks, 
                opt => opt.MapFrom(src => src.PlaylistTracks
                .OrderBy(pt => pt.Position)));

            CreateMap<PlaylistTrack, PlaylistTrackDto>();
            CreateMap<PlaylistTrack, ShowPlaylistTrackDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Track.Title))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Track.Length))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.AlbumTitle, opt => opt.MapFrom(src => src.Track.Album.Title));
        }
    }
}