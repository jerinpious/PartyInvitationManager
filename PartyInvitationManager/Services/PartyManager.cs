using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Models;
using PartyInvitationManager.Services.Interfaces;
using PartyInvitationManager.ViewModels;
using PartyInvitationManager.Services;
using PartyInvitationManager.Data;


namespace PartyInvitationManager.Services
{
    public class PartyManagerServices : IPartyManager
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public PartyManagerServices(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<PartyViewModel>> GetAllPartiesAsync()
        {
            return await _context.Parties
                .Select(p => new PartyViewModel
                {
                    PartyId = p.PartyId,
                    Description = p.Description,
                    EventDate = p.EventDate,
                    Location = p.Location,
                    InvitationCount = p.Invitations.Count
                })
                .ToListAsync();
        }

        public async Task<Party?> GetPartyByIdAsync(int id)
        {
            return await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.PartyId == id);
        }

        public async Task<ManagePartyViewModel?> GetManagePartyViewModelAsync(int id)
        {
            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.PartyId == id);

            if (party == null)
                return null;

            var viewModel = new ManagePartyViewModel
            {
                PartyId = party.PartyId,
                Description = party.Description,
                EventDate = party.EventDate,
                Location = party.Location,
                Invitations = party.Invitations.Select(i => new InvitationViewModel
                {
                    InvitationId = i.InvitationId,
                    GuestName = i.GuestName,
                    GuestEmail = i.GuestEmail,
                    Status = i.Status
                }).ToList(),
                NotSentCount = party.Invitations.Count(i => i.Status == InvitationStatus.InvitationNotSent),
                SentCount = party.Invitations.Count(i => i.Status == InvitationStatus.InvitationSent),
                YesCount = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedYes),
                NoCount = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedNo)
            };

            return viewModel;
        }

        public async Task CreatePartyAsync(CreatePartyViewModel model)
        {
            var party = new Party
            {
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location
            };

            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePartyAsync(EditPartyViewModel model)
        {
            var party = await _context.Parties.FindAsync(model.PartyId);
            if (party != null)
            {
                party.Description = model.Description;
                party.EventDate = model.EventDate;
                party.Location = model.Location;

                await _context.SaveChangesAsync();
            }
        }

        public async Task AddInvitationToPartyAsync(int partyId, string guestName, string guestEmail)
        {
            var invitation = new Invitation
            {
                PartyId = partyId,
                GuestName = guestName,
                GuestEmail = guestEmail,
                Status = InvitationStatus.InvitationNotSent
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SendInvitationsAsync(int partyId)
        {
            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.PartyId == partyId);

            if (party == null)
                return false;

            var unsentInvitations = party.Invitations
                .Where(i => i.Status == InvitationStatus.InvitationNotSent)
                .ToList();

            foreach (var invitation in unsentInvitations)
            {
                var emailSent = await _emailService.SendInvitationEmailAsync(
                    invitation.GuestEmail,
                    invitation.GuestName,
                    party.Description,
                    party.EventDate,
                    party.Location,
                    invitation.InvitationId);

                if (emailSent)
                {
                    invitation.Status = InvitationStatus.InvitationSent;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Invitation?> GetInvitationByIdAsync(int id)
        {
            return await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.InvitationId == id);
        }

        public async Task<InvitationResponseViewModel?> GetInvitationResponseViewModelAsync(int id)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.InvitationId == id);

            if (invitation == null || invitation.Party == null)
                return null;

            return new InvitationResponseViewModel
            {
                InvitationId = invitation.InvitationId,
                GuestName = invitation.GuestName,
                PartyDescription = invitation.Party.Description,
                PartyDate = invitation.Party.EventDate,
                PartyLocation = invitation.Party.Location,
                WillAttend = invitation.Status == InvitationStatus.RespondedYes ? true :
                             invitation.Status == InvitationStatus.RespondedNo ? false : null
            };
        }

        public async Task UpdateInvitationResponseAsync(int id, bool willAttend)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation != null)
            {
                invitation.Status = willAttend ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;
                await _context.SaveChangesAsync();
            }
        }
    }
}