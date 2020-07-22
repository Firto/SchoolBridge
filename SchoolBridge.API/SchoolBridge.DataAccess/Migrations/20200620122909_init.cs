using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBridge.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbbName = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStringIds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStringTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    FileId = table.Column<string>(nullable: false),
                    Static = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_Images_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStrings",
                columns: table => new
                {
                    IdId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    String = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStrings", x => new { x.IdId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_LanguageStrings_LanguageStringIds_IdId",
                        column: x => x.IdId,
                        principalTable: "LanguageStringIds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageStrings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStringIdTypes",
                columns: table => new
                {
                    StringIdId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringIdTypes", x => new { x.TypeId, x.StringIdId });
                    table.ForeignKey(
                        name: "FK_LanguageStringIdTypes_LanguageStringIds_StringIdId",
                        column: x => x.StringIdId,
                        principalTable: "LanguageStringIds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageStringIdTypes_LanguageStringTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LanguageStringTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefaultRolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultRolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_DefaultRolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefaultRolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Login = table.Column<string>(maxLength: 120, nullable: false),
                    Email = table.Column<string>(maxLength: 210, nullable: false),
                    Name = table.Column<string>(maxLength: 210, nullable: false),
                    Surname = table.Column<string>(maxLength: 210, nullable: false),
                    Lastname = table.Column<string>(maxLength: 210, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 210, nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Banned = table.Column<string>(maxLength: 210, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActiveRefreshTokens",
                columns: table => new
                {
                    Jti = table.Column<string>(nullable: false),
                    UUID = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Expire = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveRefreshTokens", x => x.Jti);
                    table.ForeignKey(
                        name: "FK_ActiveRefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Base64Sourse = table.Column<string>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Files",
                column: "Id",
                value: "default-user-photo");

            migrationBuilder.InsertData(
                table: "LanguageStringTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "client-error" },
                    { 2, "component" },
                    { 3, "valid-error" },
                    { 4, "default" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "AbbName", "FullName" },
                values: new object[,]
                {
                    { 1, "en", "English" },
                    { 2, "ua", "Українська" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 21, "CreateLanguageString" },
                    { 22, "GetLanguageString" },
                    { 23, "EditLanguageString" },
                    { 24, "RemoveLanguageString" },
                    { 25, "AddLanguageStringType" },
                    { 26, "GetLanguageStringType" },
                    { 27, "EditLanguageStringType" },
                    { 31, "GetPupilsList" },
                    { 29, "GetAdminsList" },
                    { 30, "GetAdminInfo" },
                    { 20, "RemoveLanguageStringIdType" },
                    { 32, "GetPupilsInfo" },
                    { 33, "UpdateBaseUpdateId" },
                    { 34, "GetAdminPanel" },
                    { 35, "GetGlobalizationTab" },
                    { 28, "RemoveLanguageStringType" },
                    { 19, "EditLanguageStringIdType" },
                    { 15, "EditLanguageStringId" },
                    { 17, "CreateLanguageStringIdType" },
                    { 1, "CreateAdmin" },
                    { 2, "EditAdmin" },
                    { 3, "EditAdminPermissions" },
                    { 4, "RemoveAdmin" },
                    { 5, "CreatePupil" },
                    { 6, "EditPupilPermissions" },
                    { 7, "EditPupil" },
                    { 18, "GetLanguageStringIdType" },
                    { 8, "RemovePupil" },
                    { 10, "GetLanguage" },
                    { 11, "EditLanguage" },
                    { 12, "RemoveLanguage" },
                    { 13, "CreateLanguageStringId" },
                    { 14, "GetLanguageStringId" },
                    { 16, "RemoveLanguageStringId" },
                    { 9, "CreateLanguage" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 9, "Pupil" }
                });

            migrationBuilder.InsertData(
                table: "DefaultRolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "FileId", "Static", "Type" },
                values: new object[] { "default-user-photo", true, "image/jpeg" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Banned", "Birthday", "Email", "Lastname", "Login", "Name", "PasswordHash", "RoleId", "Surname" },
                values: new object[] { "admin", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.admin", "Admin", "admin", "Admin", "26ECBVYXG5LTG6S4F7DE7CE0EAAB506AF411CABC26BE20437F9E83957412E1E501A5AEF5A1A62F1CC6528EC4F013CF617FDBDAE076", 1, "Admin" });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                values: new object[,]
                {
                    { "admin", 1 },
                    { "admin", 33 },
                    { "admin", 32 },
                    { "admin", 31 },
                    { "admin", 30 },
                    { "admin", 29 },
                    { "admin", 28 },
                    { "admin", 27 },
                    { "admin", 26 },
                    { "admin", 25 },
                    { "admin", 24 },
                    { "admin", 23 },
                    { "admin", 22 },
                    { "admin", 21 },
                    { "admin", 20 },
                    { "admin", 34 },
                    { "admin", 19 },
                    { "admin", 17 },
                    { "admin", 16 },
                    { "admin", 15 },
                    { "admin", 14 },
                    { "admin", 13 },
                    { "admin", 12 },
                    { "admin", 11 },
                    { "admin", 10 },
                    { "admin", 7 },
                    { "admin", 6 },
                    { "admin", 5 },
                    { "admin", 4 },
                    { "admin", 3 },
                    { "admin", 2 },
                    { "admin", 18 },
                    { "admin", 35 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveRefreshTokens_UserId",
                table: "ActiveRefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultRolePermissions_PermissionId",
                table: "DefaultRolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStringIdTypes_StringIdId",
                table: "LanguageStringIdTypes",
                column: "StringIdId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStrings_LanguageId",
                table: "LanguageStrings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveRefreshTokens");

            migrationBuilder.DropTable(
                name: "DefaultRolePermissions");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LanguageStringIdTypes");

            migrationBuilder.DropTable(
                name: "LanguageStrings");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "LanguageStringTypes");

            migrationBuilder.DropTable(
                name: "LanguageStringIds");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
