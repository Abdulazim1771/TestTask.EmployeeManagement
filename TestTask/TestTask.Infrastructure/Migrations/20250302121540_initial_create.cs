using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Infrastructure.Migrations;

/// <inheritdoc />
public partial class initial_create : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employee",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PayrollNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Surname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                Telephone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Address_2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                PostCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                StartDate = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employee", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Employee");
    }
}
