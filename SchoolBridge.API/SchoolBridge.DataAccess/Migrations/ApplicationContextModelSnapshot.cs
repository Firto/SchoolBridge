﻿// <auto-generated />
using System;
using SchoolBridge.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SchoolBridge.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Authorization.ActiveRefreshToken<SchoolBridge.DataAccess.Entities.User>", b =>
                {
                    b.Property<string>("Jti")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expire")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UUID")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Jti");

                    b.HasIndex("UserId");

                    b.ToTable("ActiveRefreshTokens");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Files.File", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");

                    b.HasData(
                        new
                        {
                            Id = "default-user-photo"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Files.Images.Image", b =>
                {
                    b.Property<string>("FileId")
                        .HasColumnType("text");

                    b.Property<bool>("Static")
                        .HasColumnType("boolean");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FileId");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            FileId = "default-user-photo",
                            Static = true,
                            Type = "image/jpeg"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Notification<SchoolBridge.DataAccess.Entities.User>", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Base64Sourse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Read")
                        .HasColumnType("boolean");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Panel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Panels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Pupil"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.PanelPermission", b =>
                {
                    b.Property<int>("PanelId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.HasKey("PanelId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("PanelPermissions");

                    b.HasData(
                        new
                        {
                            PanelId = 1,
                            PermissionId = 5
                        },
                        new
                        {
                            PanelId = 1,
                            PermissionId = 6
                        },
                        new
                        {
                            PanelId = 1,
                            PermissionId = 7
                        },
                        new
                        {
                            PanelId = 1,
                            PermissionId = 10
                        },
                        new
                        {
                            PanelId = 1,
                            PermissionId = 11
                        },
                        new
                        {
                            PanelId = 1,
                            PermissionId = 12
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "CreateAdmin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "EditAdmin"
                        },
                        new
                        {
                            Id = 3,
                            Name = "EditAdminPermissions"
                        },
                        new
                        {
                            Id = 4,
                            Name = "RemoveAdmin"
                        },
                        new
                        {
                            Id = 5,
                            Name = "CreatePupil"
                        },
                        new
                        {
                            Id = 6,
                            Name = "EditPupilPermissions"
                        },
                        new
                        {
                            Id = 7,
                            Name = "EditPupil"
                        },
                        new
                        {
                            Id = 8,
                            Name = "RemovePupil"
                        },
                        new
                        {
                            Id = 9,
                            Name = "GetAdminsList"
                        },
                        new
                        {
                            Id = 10,
                            Name = "GetAdminInfo"
                        },
                        new
                        {
                            Id = 11,
                            Name = "GetPupilsList"
                        },
                        new
                        {
                            Id = 12,
                            Name = "GetPupilsInfo"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Pupil"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.RolePanel", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PanelId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "PanelId");

                    b.HasIndex("PanelId");

                    b.ToTable("RolePanels");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PanelId = 1
                        },
                        new
                        {
                            RoleId = 1,
                            PanelId = 9
                        },
                        new
                        {
                            RoleId = 9,
                            PanelId = 9
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "admin",
                            Email = "admin@admin.admin",
                            Lastname = "Admin",
                            Login = "admin",
                            Name = "Admin",
                            PasswordHash = "20201VQHRBWI11D221304523052FD62A899BAD8611E2688B7005BF7E5891AF09DB129E980A6478C79650FCB2C440FEF19E59C294A4",
                            RoleId = 1,
                            Surname = "Admin"
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.UserPanel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("PanelId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "PanelId");

                    b.HasIndex("PanelId");

                    b.ToTable("UserPanels");

                    b.HasData(
                        new
                        {
                            UserId = "admin",
                            PanelId = 1
                        },
                        new
                        {
                            UserId = "admin",
                            PanelId = 9
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.UserPermission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermissions");

                    b.HasData(
                        new
                        {
                            UserId = "admin",
                            PermissionId = 1
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 2
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 3
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 4
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 5
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 6
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 7
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 10
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 11
                        },
                        new
                        {
                            UserId = "admin",
                            PermissionId = 12
                        });
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Authorization.ActiveRefreshToken<SchoolBridge.DataAccess.Entities.User>", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Files.Images.Image", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Files.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Notification<SchoolBridge.DataAccess.Entities.User>", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.PanelPermission", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Panel", "Panel")
                        .WithMany()
                        .HasForeignKey("PanelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.RolePanel", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Panel", "Panel")
                        .WithMany()
                        .HasForeignKey("PanelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.User", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.UserPanel", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Panel", "Panel")
                        .WithMany()
                        .HasForeignKey("PanelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.UserPermission", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
