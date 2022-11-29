using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_SubconDLOut_CuttingItems_SubconCuttingOutNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubconCuttingOutNo",
                table: "GarmentSubconDeliveryLetterOutItems",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubconCuttingOutNo",
                table: "GarmentSubconDeliveryLetterOutItems");
        }
    }
}
