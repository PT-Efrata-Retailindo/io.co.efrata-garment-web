using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class delete_subconcontract_sdlo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractNo",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.DropColumn(
                name: "SubconContractId",
                table: "GarmentSubconDeliveryLetterOuts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractNo",
                table: "GarmentSubconDeliveryLetterOuts",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubconContractId",
                table: "GarmentSubconDeliveryLetterOuts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
