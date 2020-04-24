using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Entities.Files;
using SchoolBridge.DataAccess.Entities.Files.Images;
using SchoolBridge.Helpers.Managers;

namespace SchoolBridge.DataAccess
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

        public DbSet<File> Files { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Panel> Panels { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PanelPermission> PanelPermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePanel> RolePanels { get; set; }
        public DbSet<UserPanel> UserPanels { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().HasKey((x) => x.FileId);
            modelBuilder.Entity<PanelPermission>().HasKey((x) => new { x.PanelId,x.PermissionId });
            modelBuilder.Entity<UserPermission>().HasKey((x) => new { x.UserId, x.PermissionId });
            modelBuilder.Entity<RolePanel>().HasKey((x) => new { x.RoleId, x.PanelId });
            modelBuilder.Entity<UserPanel>().HasKey((x) => new { x.UserId, x.PanelId });

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Admin" },
               new Role { Id = 9, Name = "Pupil" }
            );

            modelBuilder.Entity<Panel>().HasData(
                new Panel { Id = 1, Name = "Admin" },
                new Panel { Id = 9, Name = "Pupil" }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "CreateAdmin"},
                new Permission { Id = 2, Name = "EditAdmin" },
                new Permission { Id = 3, Name = "EditAdminPermissions" },
                new Permission { Id = 4, Name = "RemoveAdmin" },

                new Permission { Id = 5, Name = "CreatePupil" },
                new Permission { Id = 6, Name = "EditPupilPermissions" },
                new Permission { Id = 7, Name = "EditPupil" },
                new Permission { Id = 8, Name = "RemovePupil" },

                // Get 

                /*
                  new Panel { Id = 1, Name = "Admin" },
                new Panel { Id = 2, Name = "Moderator" },
                new Panel { Id = 3, Name = "RegionModerator" },
                new Panel { Id = 4, Name = "DistinctModerator" },
                new Panel { Id = 6, Name = "SchoolModerator" },
                new Panel { Id = 7, Name = "HeadTeacher" },
                new Panel { Id = 8, Name = "Teacher" },
                new Panel { Id = 9, Name = "Pupil" }
                 */

                new Permission { Id = 9, Name = "GetAdminsList" },
                new Permission { Id = 10, Name = "GetAdminInfo" },

                new Permission { Id = 11, Name = "GetPupilsList" },
                new Permission { Id = 12, Name = "GetPupilsInfo" }
            );

            modelBuilder.Entity<PanelPermission>().HasData(
                /*new PanelPermission { PanelId = 1, PermissionId = 1 },
                new PanelPermission { PanelId = 1, PermissionId = 2 },
                new PanelPermission { PanelId = 1, PermissionId = 3 },
                new PanelPermission { PanelId = 1, PermissionId = 4 },*/
                new PanelPermission { PanelId = 1, PermissionId = 5 },
                new PanelPermission { PanelId = 1, PermissionId = 6 },
                new PanelPermission { PanelId = 1, PermissionId = 7 },
                new PanelPermission { PanelId = 1, PermissionId = 10 },
                new PanelPermission { PanelId = 1, PermissionId = 11 },
                new PanelPermission { PanelId = 1, PermissionId = 12 }
            );

            modelBuilder.Entity<RolePanel>().HasData(
                new RolePanel { RoleId = 1, PanelId = 1 },
                new RolePanel { RoleId = 1, PanelId = 9 },
                new RolePanel { RoleId = 9, PanelId = 9 }
            );

            modelBuilder.Entity<UserPermission>().HasData(
                new UserPermission { UserId = "admin", PermissionId = 1 },
                new UserPermission { UserId = "admin", PermissionId = 2 },
                new UserPermission { UserId = "admin", PermissionId = 3 },
                new UserPermission { UserId = "admin", PermissionId = 4 },
                new UserPermission { UserId = "admin", PermissionId = 5 },
                new UserPermission { UserId = "admin", PermissionId = 6 },
                new UserPermission { UserId = "admin", PermissionId = 7 },
                new UserPermission { UserId = "admin", PermissionId = 10 },
                new UserPermission { UserId = "admin", PermissionId = 11 },
                new UserPermission { UserId = "admin", PermissionId = 12 }
            );

            modelBuilder.Entity<UserPanel>().HasData(
               new UserPanel { UserId = "admin", PanelId = 1 },
               new UserPanel { UserId = "admin", PanelId = 9 }
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
                Login = "admin",
                PasswordHash = PasswordHandler.CreatePasswordHash("admin"),
                RoleId = 1
            });

           

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