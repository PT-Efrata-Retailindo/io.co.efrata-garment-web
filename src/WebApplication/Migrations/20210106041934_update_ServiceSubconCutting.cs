using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_ServiceSubconCutting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInItemId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInItemId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.AddColumn<string>(
                name: "ContractType",
                table: "GarmentServiceSubconCuttings",
                maxLength: 25,
                nullable: true);
        }
    }
}
