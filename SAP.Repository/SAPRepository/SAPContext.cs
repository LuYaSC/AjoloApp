using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAP.Repository.SAPRepository.Entities;

namespace SAP.Repository.SAPRepository
{
    public class SAPContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SAPContext(DbContextOptions<SAPContext> options) : base(options)
        {
        }

        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Kid> Kids { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<AssignedRoom> AssignedRooms { get; set; }
        public DbSet<AssignedTutor> AssignedTutors { get; set; }
        public DbSet<KidBackground> KidBackgrounds { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
