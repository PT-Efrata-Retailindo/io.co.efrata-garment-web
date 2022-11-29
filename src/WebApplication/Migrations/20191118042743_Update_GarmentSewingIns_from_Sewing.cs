using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_GarmentSewingIns_from_Sewing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SewingFrom",
                table: "GarmentSewingIns",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutDetailId",
                table: "GarmentSewingInItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutItemId",
                table: "GarmentSewingInItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SewingFrom",
                table: "GarmentSewingIns");

            migrationBuilder.DropColumn(
                name: "SewingOutDetailId",
                table: "GarmentSewingInItems");

            migrationBuilder.DropColumn(
                name: "SewingOutItemId",
                table: "GarmentSewingInItems");
        }
    }
}
