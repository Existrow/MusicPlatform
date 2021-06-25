using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Services.Implementatiotion
{
    public class FileSaver : IFileSaver
    {
        public string Save(IFormFile file)
        {
            var guid = Guid.NewGuid();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"static", $"{guid}.mp3");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return $"/static/{guid}.mp3";
        }
    }
}
