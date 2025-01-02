using Microsoft.EntityFrameworkCore;
using LoginAppApi.Models;

namespace LoginAppApi.Data
{
    public class LoginAppDbContext : DbContext
    {
        public LoginAppDbContext(DbContextOptions<LoginAppDbContext> options) : base(options)
        {
        }

        // DbSet for Users table
        public DbSet<User> Users { get; set; }
        // DbSets represent tables in the database
        public DbSet<Access> Access { get; set; }    // Access table

        // DbSet for Screens table
        public DbSet<Screen> screens { get; set; }

       // Dbsets for Clients table

        public DbSet<Client> Clients { get; set; }

        public DbSet<AddUser> AddUsers { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Models.TaskEntity> Tasks { get; set; }

        // OnModelCreating is used to configure relationships and seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User table
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()

                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
               
                .HasMaxLength(50);

            // Configure Access table
            modelBuilder.Entity<Access>()
                .HasKey(a => a.AccessId); // Defining primary key

            modelBuilder.Entity<Access>()
                .Property(a => a.ScreenName)
                .IsRequired()
                .HasMaxLength(100); // Configuring ScreenName to be required and max length

            modelBuilder.Entity<Access>()
                .Property(a => a.Role)
                .HasMaxLength(50); // Configuring Role field to have a max length of 50 characters
                                   // Configure Screens table (add if missing)
            modelBuilder.Entity<Screen>()
                .HasKey(s => s.ScreenId);
            modelBuilder.Entity<Screen>()
                .Property(s => s.ScreenName)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
