using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBridge.DataAccess.Migrations
{
    public partial class addPupilSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PupilSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    DayNumber = table.Column<byte>(nullable: false),
                    LessonNumber = table.Column<byte>(nullable: false),
                    PupilId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PupilSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PupilSubjects_Users_PupilId",
                        column: x => x.PupilId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AD9BEAI6VUZZY+97854EDA8195339B99ED859139B03A5B8CC2DFFAC1D3DE41C88315A8887900B2562E7DF3F793378456BBAFA5786D");

            migrationBuilder.CreateIndex(
                name: "IX_PupilSubjects_PupilId",
                table: "PupilSubjects",
                column: "PupilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PupilSubjects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "26ECBVYXG5LTG6S4F7DE7CE0EAAB506AF411CABC26BE20437F9E83957412E1E501A5AEF5A1A62F1CC6528EC4F013CF617FDBDAE076");
        }
    }
}
