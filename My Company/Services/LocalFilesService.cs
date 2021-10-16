using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class LocalFilesService : IFilesService
    {
        private readonly string baseUrl;

        public LocalFilesService(IWebHostEnvironment environment)
        {
            baseUrl = Path.Combine(environment.WebRootPath, "Content");
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(baseUrl, fileName);
            using FileStream stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"/Content/{fileName}";

        }

        public string UploadFile(Bitmap image)
        {
            string fileName = $"{Guid.NewGuid()}.jpg";
            string filePath = Path.Combine(baseUrl, fileName);
            using FileStream stream = new FileStream(filePath, FileMode.Create);
            image.Save(stream, ImageFormat.Jpeg);
            return $"/Content/{fileName}";
        }

        public async Task<IEnumerable<string>> UploadFiles(IFormFileCollection files)
        {
            List<string> paths = new();
            foreach (var file in files)
            {
                paths.Add(await UploadFile(file));
            }
            return paths;
        }
    }
}
