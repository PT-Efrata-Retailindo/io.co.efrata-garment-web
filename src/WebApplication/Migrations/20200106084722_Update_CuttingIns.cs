using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_CuttingIns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuttingFrom",
                table: "GarmentCuttingIns",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutId",
                table: "GarmentCuttingInItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SewingOutNo",
                table: "GarmentCuttingInItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingFrom",
                table: "GarmentCuttingIns");

            migrationBuilder.DropColumn(
                name: "SewingOutId",
                table: "GarmentCuttingInItems");

            migrationBuilder.DropColumn(
                name: "SewingOutNo",
                table: "GarmentCuttingInItems");
        }
    }
}
