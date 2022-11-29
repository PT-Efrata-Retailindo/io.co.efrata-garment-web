using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_GarmentSubconDeliveryOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubconCuttingOutNo",
                table: "GarmentSubconDeliveryLetterOutItems",
                newName: "SubconNo");

            migrationBuilder.RenameColumn(
                name: "SubconCuttingOutId",
                table: "GarmentSubconDeliveryLetterOutItems",
                newName: "SubconId");

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "GarmentSubconDeliveryLetterOuts",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.RenameColumn(
                name: "SubconNo",
                table: "GarmentSubconDeliveryLetterOutItems",
                newName: "SubconCuttingOutNo");

            migrationBuilder.RenameColumn(
                name: "SubconId",
                table: "GarmentSubconDeliveryLetterOutItems",
                newName: "SubconCuttingOutId");
        }
    }
}
