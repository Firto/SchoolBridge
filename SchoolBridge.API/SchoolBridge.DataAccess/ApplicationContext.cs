using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Entities.Chat;
using SchoolBridge.DataAccess.Entities.Files;
using SchoolBridge.DataAccess.Entities.Files.Images;
using SchoolBridge.Helpers.Managers;

namespace SchoolBridge.DataAccess
{
    public class AuthContext<AUser> : DbContext where AUser: AuthUser
    {
        public DbSet<ActiveRefreshToken> ActiveRefreshTokens { get; set; }
        public DbSet<AUser> Users { get; set; }

        public AuthContext(DbContextOptions options) : base(options){}
    }

    public class ApplicationContext: AuthContext<User>
    {
        public DbSet<Role> Roles { get; set; }

        public DbSet<DirectChat> DirectChats { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PupilSubject> PupilSubjects { get; set; }

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

            modelBuilder.Entity<Notification>()
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

            base.OnModelCreating(modelBuilder);
        }
    }
}