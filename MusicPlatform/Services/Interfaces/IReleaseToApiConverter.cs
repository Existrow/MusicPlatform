using MusicPlatform.Models.Api.Responses;
using MusicPlatform.Models.DataBaseEntities;

namespace MusicPlatform.Services.Interfaces
{
    public interface IReleaseToApiConverter
    {
        AlbumApi Convert(Album converted, int currentUserId);
        TrackApi Convert(Track converted, int currentUserId);
    }
}