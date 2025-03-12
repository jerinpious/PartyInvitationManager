using PartyInvitationManager.Models;

namespace PartyInvitationManager.ViewModels
{
    public class PartyViewModel
    {
        public int PartyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int InvitationCount { get; set; }
    }
}