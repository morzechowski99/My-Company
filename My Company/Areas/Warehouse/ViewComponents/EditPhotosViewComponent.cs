using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using System.Collections.Generic;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class EditPhotosViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<PhotoViewModel> photos)
        {
            return View("EditPhotos", photos);
        }
    }
}
