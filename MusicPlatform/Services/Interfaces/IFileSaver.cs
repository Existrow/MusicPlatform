using Microsoft.AspNetCore.Http;

namespace MusicPlatform.Services.Interfaces
{
    public interface IFileSaver
    {
        string Save(IFormFile file);
    }
}