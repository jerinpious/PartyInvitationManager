using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyInvitationManager.Models
{
    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Declined
    }

    public class Invitation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        // Foreign Key - linking invitation to a party
        [ForeignKey("Party")]
        public int PartyId { get; set; }
        public Party? Party { get; set; }
    }
}
