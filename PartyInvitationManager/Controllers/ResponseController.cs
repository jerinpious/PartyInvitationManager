using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Services.Interfaces;

namespace PartyInvitationManager.Controllers
{
    [Route("response")]
    public class ResponseController : Controller
    {
        private readonly IPartyManager _partyManager;
        private readonly ILogger<ResponseController> _logger;

        public ResponseController(IPartyManager partyManager, ILogger<ResponseController> logger)
        {
            _partyManager = partyManager;
            _logger = logger;
        }

        [HttpGet("respond/{id:int}")]
        public async Task<IActionResult> Respond(int id)
        {
            CheckFirstVisitCookie();

            var model = await _partyManager.GetInvitationResponseViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost("respond/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, [FromForm] ViewModels.InvitationResponseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.WillAttend.HasValue)
            {
                await _partyManager.UpdateInvitationResponseAsync(id, model.WillAttend.Value);
                return View("ThankYou", model);
            }

            ModelState.AddModelError("WillAttend", "Please select whether you will attend.");
            return View(model);
        }

        private void CheckFirstVisitCookie()
        {
            if (!Request.Cookies.ContainsKey("FirstVisitDate"))
            {
                var currentDate = DateTime.Now.ToString("g");
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Append("FirstVisitDate", currentDate, cookieOptions);
                ViewBag.FirstVisit = true;
            }
            else
            {
                ViewBag.FirstVisit = false;
                ViewBag.FirstVisitDate = Request.Cookies["FirstVisitDate"];
            }
        }
    }
}