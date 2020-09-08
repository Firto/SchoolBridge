using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBridge.DataAccess.Migrations
{
    public partial class sdada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Read",
                table: "DirectChats",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Read",
                table: "DirectChats",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
