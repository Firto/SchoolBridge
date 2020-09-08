﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SchoolBridge.DataAccess;

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

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Authorization.ActiveRefreshToken", b =>
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

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Chat.DirectChat", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModify")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Read")
                        .HasColumnType("integer");

                    b.Property<string>("User1Id")
                        .HasColumnType("text");

                    b.Property<string>("User2Id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("User1Id");

                    b.HasIndex("User2Id");

                    b.ToTable("DirectChats");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Chat.DirectMessage", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Base64Source")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ChatId")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SenderId")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("DirectMessages");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.DefaultRolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("DefaultRolePermissions");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Files.File", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");
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
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AbbName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageString", b =>
                {
                    b.Property<int>("IdId")
                        .HasColumnType("integer");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<string>("String")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("LanguageStrings");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageStringId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LanguageStringIds");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageStringIdType", b =>
                {
                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.Property<int>("StringIdId")
                        .HasColumnType("integer");

                    b.HasKey("TypeId", "StringIdId");

                    b.HasIndex("StringIdId");

                    b.ToTable("LanguageStringIdTypes");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageStringType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LanguageStringTypes");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Notification", b =>
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
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.PupilSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("DayNumber")
                        .HasColumnType("smallint");

                    b.Property<byte>("LessonNumber")
                        .HasColumnType("smallint");

                    b.Property<string>("PupilId")
                        .HasColumnType("text");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PupilId");

                    b.ToTable("PupilSubjects");
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
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Banned")
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("character varying(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.Property<string>("PhotoId")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("character varying(210)")
                        .HasMaxLength(210);

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
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
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Authorization.ActiveRefreshToken", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Chat.DirectChat", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User1")
                        .WithMany()
                        .HasForeignKey("User1Id");

                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User2")
                        .WithMany()
                        .HasForeignKey("User2Id");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Chat.DirectMessage", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Chat.DirectChat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId");

                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.DefaultRolePermission", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.Role", "Role")
                        .WithMany("DefaultPermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Files.Images.Image", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Files.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageString", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.LanguageStringId", "Id")
                        .WithMany()
                        .HasForeignKey("IdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.Language", "Language")
                        .WithMany("Strings")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.LanguageStringIdType", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.LanguageStringId", "StringId")
                        .WithMany("Types")
                        .HasForeignKey("StringIdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolBridge.DataAccess.Entities.LanguageStringType", "Type")
                        .WithMany("Strings")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.Notification", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.PupilSubject", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.User", "Pupil")
                        .WithMany()
                        .HasForeignKey("PupilId");
                });

            modelBuilder.Entity("SchoolBridge.DataAccess.Entities.User", b =>
                {
                    b.HasOne("SchoolBridge.DataAccess.Entities.Files.Images.Image", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("SchoolBridge.DataAccess.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
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
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
