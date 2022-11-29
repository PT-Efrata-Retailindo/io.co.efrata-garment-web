using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addadjustmentitemIdAndIdInFinishiedGoodStockHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdjustmentId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdjustmentItemId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "AdjustmentItemId",
                table: "GarmentFinishedGoodStockHistories");
        }
    }
}
