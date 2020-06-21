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
        public DbSet<Role> Roles { get; set; }

        public DbSet<Notification<User>> Notifications { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<DefaultRolePermission> DefaultRolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageStringId> LanguageStringIds { get; set; }
        public DbSet<LanguageStringType> LanguageStringTypes { get; set; }
        public DbSet<LanguageStringIdType> LanguageStringIdTypes { get; set; }
        public DbSet<LanguageString> LanguageStrings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keys

            modelBuilder.Entity<Image>().HasKey((x) => x.FileId);

            modelBuilder.Entity<DefaultRolePermission>().HasKey((x) => new { x.RoleId, x.PermissionId });
            modelBuilder.Entity<UserPermission>().HasKey((x) => new { x.UserId, x.PermissionId });

            modelBuilder.Entity<LanguageString>().HasKey((x) => new { x.IdId, x.LanguageId });
            modelBuilder.Entity<LanguageStringIdType>().HasKey((x) => new { x.TypeId, x.StringIdId });

            //

            modelBuilder.Entity<LanguageString>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Strings);

            modelBuilder.Entity<LanguageStringIdType>()
                .HasOne(x => x.StringId)
                .WithMany(x => x.Types);

            modelBuilder.Entity<LanguageStringIdType>()
               .HasOne(x => x.Type)
               .WithMany(x => x.Strings);

            modelBuilder.Entity<Notification<User>>()
               .HasOne((x) => x.User)
               .WithMany(x => x.Notifications);

            modelBuilder.Entity<UserPermission>()
               .HasOne((x) => x.User)
               .WithMany(x => x.Permissions);

            modelBuilder.Entity<DefaultRolePermission>()
               .HasOne((x) => x.Role)
               .WithMany(x => x.DefaultPermissions);

            modelBuilder.Entity<User>()
               .HasOne(t => t.Role)
               .WithMany(t => t.Users);

            // Seed data

            modelBuilder.Entity<LanguageStringType>().HasData(
                new LanguageStringType { Id = 1, Name = "client-error"},
                new LanguageStringType { Id = 2, Name = "component" },
                new LanguageStringType { Id = 3, Name = "valid-error" },
                new LanguageStringType { Id = 4, Name = "default" }
            );

            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, AbbName = "en", FullName = "English" },
                new Language { Id = 2, AbbName = "ua", FullName = "Українська" }
            );

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Admin" },
               new Role { Id = 9, Name = "Pupil" }
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

                // Languages

                new Permission { Id = 9, Name = "CreateLanguage" },
                new Permission { Id = 10, Name = "GetLanguage" },
                new Permission { Id = 11, Name = "EditLanguage" },
                new Permission { Id = 12, Name = "RemoveLanguage" },

                new Permission { Id = 13, Name = "CreateLanguageStringId" },
                new Permission { Id = 14, Name = "GetLanguageStringId" },
                new Permission { Id = 15, Name = "EditLanguageStringId" },
                new Permission { Id = 16, Name = "RemoveLanguageStringId" },

                new Permission { Id = 17, Name = "CreateLanguageStringIdType" },
                new Permission { Id = 18, Name = "GetLanguageStringIdType" },
                new Permission { Id = 19, Name = "EditLanguageStringIdType" },
                new Permission { Id = 20, Name = "RemoveLanguageStringIdType" },

                new Permission { Id = 21, Name = "CreateLanguageString" },
                new Permission { Id = 22, Name = "GetLanguageString" },
                new Permission { Id = 23, Name = "EditLanguageString" },
                new Permission { Id = 24, Name = "RemoveLanguageString" },

                new Permission { Id = 25, Name = "AddLanguageStringType" },
                new Permission { Id = 26, Name = "GetLanguageStringType" },
                new Permission { Id = 27, Name = "EditLanguageStringType" },
                new Permission { Id = 28, Name = "RemoveLanguageStringType" },

                new Permission { Id = 29, Name = "GetAdminsList" },
                new Permission { Id = 30, Name = "GetAdminInfo" },

                new Permission { Id = 31, Name = "GetPupilsList" },
                new Permission { Id = 32, Name = "GetPupilsInfo" },

                new Permission { Id = 33, Name = "UpdateBaseUpdateId" },
                new Permission { Id = 34, Name = "GetAdminPanel" },
                new Permission { Id = 35, Name = "GetGlobalizationTab" }
            );

            modelBuilder.Entity<DefaultRolePermission>().HasData(
                /*new PanelPermission { PanelId = 1, PermissionId = 1 },
                new PanelPermission { PanelId = 1, PermissionId = 2 },
                new PanelPermission { PanelId = 1, PermissionId = 3 },
                new PanelPermission { PanelId = 1, PermissionId = 4 },*/
                new DefaultRolePermission { RoleId = 1, PermissionId = 5 },
                new DefaultRolePermission { RoleId = 1, PermissionId = 6 },
                new DefaultRolePermission { RoleId = 1, PermissionId = 7 },
                new DefaultRolePermission { RoleId = 1, PermissionId = 10 },
                new DefaultRolePermission { RoleId = 1, PermissionId = 11 },
                new DefaultRolePermission { RoleId = 1, PermissionId = 12 }
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
                new UserPermission { UserId = "admin", PermissionId = 12 },
                new UserPermission { UserId = "admin", PermissionId = 13 },
                new UserPermission { UserId = "admin", PermissionId = 14 },
                new UserPermission { UserId = "admin", PermissionId = 15 },
                new UserPermission { UserId = "admin", PermissionId = 16 },
                new UserPermission { UserId = "admin", PermissionId = 17 },
                new UserPermission { UserId = "admin", PermissionId = 18 },
                new UserPermission { UserId = "admin", PermissionId = 19 },
                new UserPermission { UserId = "admin", PermissionId = 20 },
                new UserPermission { UserId = "admin", PermissionId = 21 },
                new UserPermission { UserId = "admin", PermissionId = 22 },
                new UserPermission { UserId = "admin", PermissionId = 23 },
                new UserPermission { UserId = "admin", PermissionId = 24 },
                new UserPermission { UserId = "admin", PermissionId = 25 },
                new UserPermission { UserId = "admin", PermissionId = 26 },
                new UserPermission { UserId = "admin", PermissionId = 27 },
                new UserPermission { UserId = "admin", PermissionId = 28 },
                new UserPermission { UserId = "admin", PermissionId = 29 },
                new UserPermission { UserId = "admin", PermissionId = 30 },
                new UserPermission { UserId = "admin", PermissionId = 31 },
                new UserPermission { UserId = "admin", PermissionId = 32 },
                new UserPermission { UserId = "admin", PermissionId = 33 },
                new UserPermission { UserId = "admin", PermissionId = 34 },
                new UserPermission { UserId = "admin", PermissionId = 35 }
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


            base.OnModelCreating(modelBuilder);
        }
    }
}