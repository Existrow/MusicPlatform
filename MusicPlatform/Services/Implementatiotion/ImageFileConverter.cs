using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Services.Implementatiotion
{
    public class ImageFileConverter : IImageFileConverter
    {
        public byte[] ConvertToByte(IFormFile file)
        {
            if (file is not null)
                using (var filestream = file.OpenReadStream())
                {
                    using (var binaryReader = new BinaryReader(filestream))
                    {
                        return binaryReader.ReadBytes((int)file.Length);
                    }
                }
            else
                return null;
        }

        public string ConvertToString(byte[] bytes)
            => bytes is not null
                ? "data:image/jpeg;base64," + Convert.ToBase64String(bytes)
                : null;
    }
}
