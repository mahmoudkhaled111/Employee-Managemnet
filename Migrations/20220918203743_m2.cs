using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeMangement.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Eployees",
                columns: new[] { "Id", "Department", "Email", "Name" },
                values: new object[] { 2, 1, "mahmoudkhaledsalama@gmail.com", "khaled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Eployees",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
