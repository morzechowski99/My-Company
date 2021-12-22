using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IFilesService
    {
        Task<IEnumerable<string>> UploadFiles(IFormFileCollection files);
        Task<string> UploadFile(IFormFile file);
        string UploadFile(Bitmap image);
        void DeletePhoto(string path);
        Task<string> ChangeShopLogo(IFormFile logo);
    }
}
