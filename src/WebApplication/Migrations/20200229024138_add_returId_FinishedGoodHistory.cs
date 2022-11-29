using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_returId_FinishedGoodHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExpenditureGoodReturnId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenditureGoodReturnItemId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenditureGoodReturnId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "ExpenditureGoodReturnItemId",
                table: "GarmentFinishedGoodStockHistories");
        }
    }
}
