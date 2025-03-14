﻿using PartyInvitationManager.Models;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models
{
    public class Party
    {
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        public string Location { get; set; } = string.Empty;

        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}