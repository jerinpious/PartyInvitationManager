using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Models;

namespace PartyInvitationManager.Controllers
{
    public class InvitationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvitationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invitation
        public async Task<IActionResult> Index()
        {
            var invitations = await _context.Invitations.Include(i => i.Party).ToListAsync();
            return View(invitations);
        }

        // GET: Invitation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invitation == null)
                return NotFound();

            return View(invitation);
        }

        // GET: Invitation/Create
        public IActionResult Create()
        {
            ViewBag.Parties = _context.Parties.ToList();  // Populate dropdown
            return View();
        }

        // POST: Invitation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GuestName,GuestEmail,PartyId,Status")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parties = _context.Parties.ToList();  // Repopulate dropdown in case of error
            return View(invitation);
        }

        // GET: Invitation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null)
                return NotFound();

            ViewBag.Parties = _context.Parties.ToList();  // Populate dropdown
            return View(invitation);
        }

        // POST: Invitation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GuestName,GuestEmail,PartyId,Status")] Invitation invitation)
        {
            if (id != invitation.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Invitations.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parties = _context.Parties.ToList();
            return View(invitation);
        }

        // GET: Invitation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invitation == null)
                return NotFound();

            return View(invitation);
        }

        // POST: Invitation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation != null)
            {
                _context.Invitations.Remove(invitation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
    