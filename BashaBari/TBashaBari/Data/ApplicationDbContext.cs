using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBashaBari.Models;

namespace TBashaBari.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet <OwnerNotice> OwnerNotice { get; set; }
        public DbSet <TenantRequest> TenantRequest { get; set; }
        public DbSet <ApplicationUser> ApplicationUser { get; set; }
        public DbSet <TenantConnectsOwner> TenantConnectsOwner { get; set; }
        public DbSet <BillInformation> BillInformation { get; set; }
    }
}
