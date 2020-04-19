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
               new Role { Id = 6, Name = "SchoolModerator" },
               new Role { Id = 7, Name = "HeadTeacher" },
               new Role { Id = 8, Name = "Teacher" },
               new Role { Id = 9, Name = "Pupil" }
            );

            modelBuilder.Entity<Panel>().HasData(
                new Panel { Id = 1, Name = "Admin" },
                new Panel { Id = 2, Name = "Moderator" },
                new Panel { Id = 3, Name = "RegionModerator" },
                new Panel { Id = 4, Name = "DistinctModerator" },
                new Panel { Id = 6, Name = "SchoolModerator" },
                new Panel { Id = 7, Name = "HeadTeacher" },
                new Panel { Id = 8, Name = "Teacher" },
                new Panel { Id = 9, Name = "Pupil" }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "CreateAdmin"},
                new Permission { Id = 1, Name = "EditMyAdmin" },
                new Permission { Id = 1, Name = "EditOtherAdmin" },
                new Permission { Id = 1, Name = "EditOtherAdminPermissions" },
                new Permission { Id = 1, Name = "RemoveMyAdmin" },
                new Permission { Id = 1, Name = "RemoveOtherAdmin" },

                new Permission { Id = 1, Name = "CreateModerator" },
                new Permission { Id = 1, Name = "EditMyModerator" },
                new Permission { Id = 1, Name = "EditOtherModerator" },
                new Permission { Id = 1, Name = "EditOtherModeratorPermissions" },
                new Permission { Id = 1, Name = "RemoveMyModerator" },
                new Permission { Id = 1, Name = "RemoveOtherModerator" },

                new Permission { Id = 1, Name = "CreateRegionModerator" },
                new Permission { Id = 1, Name = "EditMyRegionModerator" },
                new Permission { Id = 1, Name = "EditOtherRegionModerator" },
                new Permission { Id = 1, Name = "EditOtherRegionModerator" },
                new Permission { Id = 1, Name = "RemoveMyRegionModerator" },
                new Permission { Id = 1, Name = "RemoveOtherRegionModerator" },

                new Permission { Id = 1, Name = "CreateDistinctModerator" },
                new Permission { Id = 1, Name = "EditMyDistinctModerator" },
                new Permission { Id = 1, Name = "EditOtherDistinctModerator" },
                new Permission { Id = 1, Name = "EditOtherDistinctModerator" },
                new Permission { Id = 1, Name = "RemoveMyDistinctModerator" },
                new Permission { Id = 1, Name = "RemoveOtherDistinctModerator" },

                new Permission { Id = 1, Name = "CreateSchoolModerator" },
                new Permission { Id = 1, Name = "EditMySchoolModerator" },
                new Permission { Id = 1, Name = "EditOtherSchoolModerator" },
                new Permission { Id = 1, Name = "EditOtherSchoolModerator" },
                new Permission { Id = 1, Name = "RemoveMySchoolModerator" },
                new Permission { Id = 1, Name = "RemoveOtherSchoolModerator" },

                new Permission { Id = 1, Name = "CreateHeadTeacher" },
                new Permission { Id = 1, Name = "EditMyHeadTeacher" },
                new Permission { Id = 1, Name = "EditOtherHeadTeacher" },
                new Permission { Id = 1, Name = "EditOtherHeadTeacher" },
                new Permission { Id = 1, Name = "RemoveMyHeadTeacher" },
                new Permission { Id = 1, Name = "RemoveOtherHeadTeacher" },

                new Permission { Id = 1, Name = "CreateTeacher" },
                new Permission { Id = 1, Name = "EditMyTeacher" },
                new Permission { Id = 1, Name = "EditOtherTeacher" },
                new Permission { Id = 1, Name = "EditOtherTeacher" },
                new Permission { Id = 1, Name = "RemoveMyTeacher" },
                new Permission { Id = 1, Name = "RemoveOtherTeacher" },

                new Permission { Id = 1, Name = "CreatePupil" },
                new Permission { Id = 1, Name = "EditMyPupil" },
                new Permission { Id = 1, Name = "EditOtherPupil" },
                new Permission { Id = 1, Name = "EditOtherPupil" },
                new Permission { Id = 1, Name = "RemoveMyPupil" },
                new Permission { Id = 1, Name = "RemoveOtherPupil" },

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

                new Permission { Id = 1, Name = "GetAdminsList" },
                new Permission { Id = 1, Name = "GetAdminInfo" },
                new Permission { Id = 1, Name = "GetMyAdminsList" },
                new Permission { Id = 1, Name = "GetMyAdminInfo" },

                new Permission { Id = 1, Name = "GetModeratorsList" },
                new Permission { Id = 1, Name = "GetModeratorInfo" },
                new Permission { Id = 1, Name = "GetMyModeratorsList" },
                new Permission { Id = 1, Name = "GetMyModeratorInfo" },

                new Permission { Id = 1, Name = "GetRegionModeratorsList" },
                new Permission { Id = 1, Name = "GetRegionModeratorsInfo" },
                new Permission { Id = 1, Name = "GetMyRegionModeratorsList" },
                new Permission { Id = 1, Name = "GetMyRegionModeratorsInfo" },

                new Permission { Id = 1, Name = "GetDistinctModeratorsList" },
                new Permission { Id = 1, Name = "GetDistinctModeratorsInfo" },
                new Permission { Id = 1, Name = "GetMyDistinctModeratorsList" },
                new Permission { Id = 1, Name = "GetMyDistinctModeratorsInfo" },

                new Permission { Id = 1, Name = "GetHeadTeachersList" },
                new Permission { Id = 1, Name = "GetHeadTeachersInfo" },
                new Permission { Id = 1, Name = "GetMyHeadTeachersList" },
                new Permission { Id = 1, Name = "GetMyHeadTeachersInfo" },

                new Permission { Id = 1, Name = "GetTeachersList" },
                new Permission { Id = 1, Name = "GetTeachersInfo" },
                new Permission { Id = 1, Name = "GetMyTeachersList" },
                new Permission { Id = 1, Name = "GetMyTeachersInfo" },
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