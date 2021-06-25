using System.Linq;
using MusicPlatform.Models.Api.Responses;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Services.Implementatiotion
{
    public class ReleaseToApiConverter : IReleaseToApiConverter
    {
        private readonly PlatformContext _context;
        private readonly IImageFileConverter _converter;

        public ReleaseToApiConverter(PlatformContext context, IImageFileConverter converter)
            => (_context, _converter) = (context, converter);

        public TrackApi Convert(Track converted, int currentUserId) => new()
        {
            Id = converted.Id,
            ReleaseData = converted.ReleaseData,
            Cover = _converter.ConvertToString(converted.Cover),
            Authors = converted.Artists.Select(artist => new { artist.Name, artist.Id}),
            Featuring = converted.Featuring.Select(artist => new { artist.Name, artist.Id }),
            Genre = converted.Genre,
            GenreId = converted.GenreId,
            IsUserLiked = IsUserLike(converted.Id, currentUserId),
            Likes = converted.UsersLikes.Count,
            Listnings = converted.Listnings,
            Title = converted.Title,
            ClipUrl = converted.ClipUrl,
            FilePath = converted.FilePath
        };


        public AlbumApi Convert(Album converted, int currentUserId) => new()
        {
            Id = converted.Id,
            Tracks = converted.Tracks.Select(track => Convert(track, currentUserId)),
            ReleaseData = converted.ReleaseData,
            Cover = _converter.ConvertToString(converted.Cover),
            Authors = converted.Artists.Select(artist => new { artist.Name, artist.Id }),
            Genre = converted.Genre,
            GenreId = converted.GenreId,
            IsUserLiked = IsUserLike(converted.Id, currentUserId),
            Likes = converted.UsersLikes.Count,
            Listnings = converted.Listnings,
            Title = converted.Title,
            Description = converted.Description
        };

        private bool IsUserLike(int releaseId, int userId)
        {
            var isliked = _context.Users.Find(userId)?.LikedReleases
                    ?.Where(release => release.Id == releaseId)
                    .Any();
            return isliked is not null
                ? isliked.Value
                : false;
        }
    }
}
