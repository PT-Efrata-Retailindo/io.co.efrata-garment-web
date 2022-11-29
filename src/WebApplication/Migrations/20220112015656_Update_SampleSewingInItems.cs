using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_SampleSewingInItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinishingOutDetailId",
                table: "GarmentSampleSewingInItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FinishingOutItemId",
                table: "GarmentSampleSewingInItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishingOutDetailId",
                table: "GarmentSampleSewingInItems");

            migrationBuilder.DropColumn(
                name: "FinishingOutItemId",
                table: "GarmentSampleSewingInItems");
        }
    }
}
