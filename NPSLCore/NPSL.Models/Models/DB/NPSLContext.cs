using Microsoft.EntityFrameworkCore;


namespace NPSLCore.Models.DB
{
    public partial class NPSLContext : DbContext
    {
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<MenuModels> MenuModels { get; set; }

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

                entity.Property(e => e.MobilePhone).IsRequired();
            });

            modelBuilder.Entity<MenuModels>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.MainMenuName).IsRequired();

                entity.Property(e => e.SubMenuName).IsRequired();

            
            });
        }
    }
}
