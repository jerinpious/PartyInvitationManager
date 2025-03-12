using PartyInvitationManager.Models;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.ViewModels
{
    public class ManagePartyViewModel
    {
        public int PartyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;

        public List<InvitationViewModel> Invitations { get; set; } = new List<InvitationViewModel>();

        [Required(ErrorMessage = "Guest name is required")]
        [Display(Name = "Guest Name")]
        public string NewGuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Guest email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Guest Email")]
        public string NewGuestEmail { get; set; } = string.Empty;

        // Statistics
        public int NotSentCount { get; set; }
        public int SentCount { get; set; }
        public int YesCount { get; set; }
        public int NoCount { get; set; }
    }

    public class InvitationViewModel
    {
        public int InvitationId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public InvitationStatus Status { get; set; }
        public string StatusText => Status.ToString();
    }
}