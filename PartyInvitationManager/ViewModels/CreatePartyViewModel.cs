using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.ViewModels
{
    public class CreatePartyViewModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);

        public string Location { get; set; } = string.Empty;
    }
}