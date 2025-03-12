using PartyInvitationManager.Models;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models
{
    public class Invitation
    {
        public int InvitationId { get; set; }

        [Required(ErrorMessage = "Guest name is required")]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Guest email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Guest Email")]
        public string GuestEmail { get; set; } = string.Empty;

        public InvitationStatus Status { get; set; } = InvitationStatus.InvitationNotSent;

        // Foreign key
        public int PartyId { get; set; }


        public Party? Party { get; set; }
    }
}