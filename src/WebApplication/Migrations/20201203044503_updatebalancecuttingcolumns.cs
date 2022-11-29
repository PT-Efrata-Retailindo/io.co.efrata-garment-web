using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updatebalancecuttingcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "GarmentBalanceCuttings");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentBalanceCuttings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GarmentBalanceCuttings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentBalanceCuttings",
                maxLength: 50,
                nullable: true);
        }
    }
}
