using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Services.Interfaces;
using PartyInvitationManager.ViewModels;

namespace PartyInvitationManager.Controllers
{
    [Route("parties")]
    public class PartyController : Controller
    {
        private readonly IPartyManager _partyManager;
        private readonly ILogger<PartyController> _logger;

        public PartyController(IPartyManager partyManager, ILogger<PartyController> logger)
        {
            _partyManager = partyManager;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            CheckFirstVisitCookie();

            var parties = await _partyManager.GetAllPartiesAsync();
            return View(parties);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            CheckFirstVisitCookie();

            return View(new CreatePartyViewModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePartyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _partyManager.CreatePartyAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Manage(int id)
        {
            CheckFirstVisitCookie();

            var model = await _partyManager.GetManagePartyViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost("{id:int}/invitations")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvitation(int id, ManagePartyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var fullModel = await _partyManager.GetManagePartyViewModelAsync(id);
                if (fullModel == null)
                {
                    return NotFound();
                }

                fullModel.NewGuestName = model.NewGuestName;
                fullModel.NewGuestEmail = model.NewGuestEmail;
                return View("Manage", fullModel);
            }

            await _partyManager.AddInvitationToPartyAsync(id, model.NewGuestName, model.NewGuestEmail);
            return RedirectToAction(nameof(Manage), new { id });
        }

        [HttpPost("{id:int}/send-invitations-requests")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvitations(int id)
        {
            await _partyManager.SendInvitationsAsync(id);
            return RedirectToAction(nameof(Manage), new { id });
        }

        [HttpGet("{id:int}/edit")]
        public async Task<IActionResult> Edit(int id)
        {

            CheckFirstVisitCookie();

            var party = await _partyManager.GetPartyByIdAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            var model = new EditPartyViewModel
            {
                PartyId = party.PartyId,
                Description = party.Description,
                EventDate = party.EventDate,
                Location = party.Location
            };

            return View(model);
        }

        [HttpPost("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPartyViewModel model)
        {
            if (id != model.PartyId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _partyManager.UpdatePartyAsync(model);
            return RedirectToAction(nameof(Manage), new { id });
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