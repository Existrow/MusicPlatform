using Microsoft.AspNetCore.Http;

namespace MusicPlatform.Services.Interfaces
{
    public interface IImageFileConverter
    {
        byte[] ConvertToByte(IFormFile file);
        string ConvertToString(byte[] file);
    }
}