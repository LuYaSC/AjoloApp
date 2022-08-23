using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAP.Repository.SAPRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository
{
    public class SAPContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SAPContext(DbContextOptions<SAPContext> options) : base(options)
        {
        }

        public DbSet<Kid> Kids { get; set; }
        public DbSet<Parent> Parents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
