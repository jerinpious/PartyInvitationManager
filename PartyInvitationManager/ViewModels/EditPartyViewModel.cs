using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.ViewModels
{
    public class EditPartyViewModel
    {
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        public string Location { get; set; } = string.Empty;
    }
}