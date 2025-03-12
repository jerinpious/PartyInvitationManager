using Microsoft.AspNetCore.Mvc;

namespace PartyManagerApp.Controllers
{
    public class InvitationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
