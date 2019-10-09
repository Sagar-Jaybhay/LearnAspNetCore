using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnAspCore.Migrations
{
    public partial class InitialSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Address", "Division", "FullName" },
                values: new object[] { 1, "abcd efgh", 4, "abc" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Address", "Division", "FullName" },
                values: new object[] { 2, "werr", 4, "xyz" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2);
        }
    }
}
