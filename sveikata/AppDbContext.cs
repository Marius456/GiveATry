using Microsoft.EntityFrameworkCore;
using giveatry.Models;
using System.Reflection;

namespace giveatry
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<UserTracker> UserTrackers { get; set; }
        public DbSet<PlanExercises> PlanExercises { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
