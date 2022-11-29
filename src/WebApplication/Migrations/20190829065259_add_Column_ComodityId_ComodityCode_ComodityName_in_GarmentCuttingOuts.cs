using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_Column_ComodityId_ComodityCode_ComodityName_in_GarmentCuttingOuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comodity",
                table: "GarmentCuttingOuts",
                newName: "ComodityName");

            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentCuttingOuts",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentCuttingOuts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentCuttingOuts");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentCuttingOuts");

            migrationBuilder.RenameColumn(
                name: "ComodityName",
                table: "GarmentCuttingOuts",
                newName: "Comodity");
        }
    }
}
