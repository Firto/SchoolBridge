using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBridge.DataAccess.Migrations
{
    public partial class setsas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "D43CFUSAL0DGWR/99174795BB6041169FB13113E6D5C328308EE22F28B8E44477DFB99F828BA08D426F72F65CD1DCE71AABAB9B82E");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "20201VQHRBWI11D221304523052FD62A899BAD8611E2688B7005BF7E5891AF09DB129E980A6478C79650FCB2C440FEF19E59C294A4");
        }
    }
}
