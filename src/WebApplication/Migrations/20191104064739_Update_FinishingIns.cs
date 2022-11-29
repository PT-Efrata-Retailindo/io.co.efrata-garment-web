using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_FinishingIns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SewingOutId",
                table: "GarmentFinishingIns");

            migrationBuilder.DropColumn(
                name: "SewingOutNo",
                table: "GarmentFinishingIns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutId",
                table: "GarmentFinishingIns",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SewingOutNo",
                table: "GarmentFinishingIns",
                maxLength: 25,
                nullable: true);
        }
    }
}
