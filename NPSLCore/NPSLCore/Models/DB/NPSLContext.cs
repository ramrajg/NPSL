using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NPSLCore.Models.DB
{
    public partial class NPSLContext : DbContext
    {
        public virtual DbSet<Users> Users { get; set; }

        public NPSLContext(DbContextOptions<NPSLContext> options)
: base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.LoginId).IsRequired();

                entity.Property(e => e.LoginPassword).IsRequired();

                entity.Property(e => e.MobilePhone).IsRequired();
            });
        }
    }
}
