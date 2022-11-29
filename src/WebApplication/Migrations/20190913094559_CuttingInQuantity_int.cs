using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class CuttingInQuantity_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CuttingInQuantity",
                table: "GarmentCuttingInDetails",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CuttingInQuantity",
                table: "GarmentCuttingInDetails",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
