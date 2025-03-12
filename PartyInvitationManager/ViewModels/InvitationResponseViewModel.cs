using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.ViewModels
{
    public class InvitationResponseViewModel
    {
        public int InvitationId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string PartyDescription { get; set; } = string.Empty;
        public DateTime PartyDate { get; set; }
        public string PartyLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a response")]
        [Display(Name = "Will you attend?")]
        public bool? WillAttend { get; set; }
    }
}