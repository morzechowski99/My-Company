using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class EditPhotosViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<PhotoViewModel> photos)
        {
            return View("EditPhotos",photos);
        }
    }
}
