using Microsoft.EntityFrameworkCore;

namespace PartyInvitationManager.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}
