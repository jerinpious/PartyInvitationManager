using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PartyInvitationManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; } = null!;
        public DbSet<Invitation> Invitations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Party>()
                .HasMany(p => p.Invitations)
                .WithOne(i => i.Party)
                .HasForeignKey(i => i.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invitation>()
                .Property(i => i.Status)
                .HasConversion<string>()
                .HasMaxLength(64);
        }
    }
}