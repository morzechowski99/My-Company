using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Helpers
{
    public static class PhotosHelpers
    {
        public static Bitmap GetResizedImage(IFormFile file, int height = 0, int width = 0)
        {
            var image = Image.FromStream(file.OpenReadStream());
            return Resize(image, height, width);
        }

        public static Bitmap GetResizedImage(string path, int height = 0, int width = 0)
        {
            var image = Image.FromStream(new FileStream(path, FileMode.Open));
            return Resize(image, height, width);
        }

        private static Bitmap Resize(Image image, int height, int width)
        {
            var h = image.Height;
            var w = image.Width;
            if (height == 0 && width == 0)
            {
                throw new ArgumentOutOfRangeException("One of dimenions must be given");
            }
            else if (height == 0)
                height = (int)(h * ((double)width / (double)w));
            else if (width == 0)
                width = (int)(w * ((double)height / (double)h));
            var resized = new Bitmap(image, new Size(width, height));
            return resized;
        }
    }
}
