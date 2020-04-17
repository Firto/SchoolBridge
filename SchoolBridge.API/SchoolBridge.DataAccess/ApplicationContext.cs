using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Entities.Files;
using SchoolBridge.DataAccess.Entities.Files.Images;
using SchoolBridge.Helpers.Managers;

namespace GreenP.DataAccess
{
    public class AuthContext<AUser> : DbContext where AUser: AuthUser
    {
        public DbSet<ActiveRefreshToken<AUser>> ActiveRefreshTokens { get; set; }
        public DbSet<AUser> Users { get; set; }

        public AuthContext(DbContextOptions options) : base(options){}
    }

    public class ApplicationContext: AuthContext<User>
    {
        public DbSet<Notification<User>> Notifications { get; set; }

        public DbSet<Image> Photos { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Moderator" },
                new Role { Id = 3, Name = "RegionModerator" },
                new Role { Id = 4, Name = "DistinctModerator" },
                new Role { Id = 5, Name = "Director" },
                new Role { Id = 6, Name = "SchoolModerator" },
                new Role { Id = 7, Name = "Teacher" },
                new Role { Id = 8, Name = "Pupil" }
            );

            modelBuilder.Entity<File>().HasData(
               new File { Id = "default-user-photo"}
           );

            modelBuilder.Entity<Image>().HasData(
                new Image { FileId = "default-user-photo", Type = "image/jpeg", Static = true }
            );

            modelBuilder.Entity<User>().HasData(new User { 
                Id = "admin",
                Name = "Admin",
                Surname = "Admin",
                Lastname = "Admin",
                Email = "admin@admin.admin",
                EmailConfirmed = true,
                Login = "admin",
                PasswordHash = PasswordHandler.CreatePasswordHash("admin"),
                PhotoId = "default-user-photo",
                RoleId = 1
            });

            modelBuilder.Entity<Image>().HasKey((x) => x.FileId);

            modelBuilder.Entity<Notification<User>>()
               .HasOne((x) => x.User)
               .WithMany(x => x.Notifications);

            modelBuilder.Entity<User>()
               .HasOne(t => t.Role)
               .WithMany(t => t.Users);


            base.OnModelCreating(modelBuilder);
        }
    }
}