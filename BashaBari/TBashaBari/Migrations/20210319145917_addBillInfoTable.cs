using Microsoft.EntityFrameworkCore.Migrations;

namespace TBashaBari.Migrations
{
    public partial class addBillInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillInformation",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterVerified = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectricAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectricPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectricVerified = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentVerified = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GasAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GasPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GasVerified = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillInformation", x => x.BillId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillInformation");
        }
    }
}
