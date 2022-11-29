using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_ServiceSubconCuttingItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CuttingInItemId",
                table: "GarmentServiceSubconCuttingItems",
                newName: "CuttingInDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingItems",
                newName: "CuttingInItemId");
        }
    }
}
