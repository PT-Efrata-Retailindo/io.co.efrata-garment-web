using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_EPO_subcon_delivery_letter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EPOId",
                table: "GarmentSubconDeliveryLetterOuts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EPONo",
                table: "GarmentSubconDeliveryLetterOuts",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPOId",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.DropColumn(
                name: "EPONo",
                table: "GarmentSubconDeliveryLetterOuts");
        }
    }
}
