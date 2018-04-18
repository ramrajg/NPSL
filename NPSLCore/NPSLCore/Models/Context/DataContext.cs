using Microsoft.EntityFrameworkCore;
using NPSLCore.Models;

namespace NPSLCore.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options) { }
       
        #region DbSets
        public DbSet<Users> Users { get; set; }
        #endregion

    }
}
