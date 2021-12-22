using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class HomeController : Controller
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public HomeController(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IActionResult> Index()
        {
            var items = await config.GetMainPageContent(repositoryWrapper.ConfigRepository);
            return View(items.OrderBy(i => i.Order).ToList());
        }
    }
}
