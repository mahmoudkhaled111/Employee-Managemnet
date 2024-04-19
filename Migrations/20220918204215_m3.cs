using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMangement.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Eployees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "khaled@gmail.com");

            migrationBuilder.InsertData(
                table: "Eployees",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 3, 1, "salama@gmail.com", "salama" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Eployees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Eployees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "mahmoudkhaledsalama@gmail.com");
        }
    }
}
