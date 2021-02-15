using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles =Role.Administrator)]
    public class HomeController : Controller
    {
        public IActionResult Index => View();
    }
}
