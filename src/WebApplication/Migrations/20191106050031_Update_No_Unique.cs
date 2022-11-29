using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_No_Unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GarmentSewingOuts_SewingOutNo",
                table: "GarmentSewingOuts",
                column: "SewingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSewingIns_SewingInNo",
                table: "GarmentSewingIns",
                column: "SewingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSewingDOs_SewingDONo",
                table: "GarmentSewingDOs",
                column: "SewingDONo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentLoadings_LoadingNo",
                table: "GarmentLoadings",
                column: "LoadingNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinishingIns_FinishingInNo",
                table: "GarmentFinishingIns",
                column: "FinishingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentDeliveryReturns_DRNo",
                table: "GarmentDeliveryReturns",
                column: "DRNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentCuttingOuts_CutOutNo",
                table: "GarmentCuttingOuts",
                column: "CutOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentSewingOuts_SewingOutNo",
                table: "GarmentSewingOuts");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSewingIns_SewingInNo",
                table: "GarmentSewingIns");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSewingDOs_SewingDONo",
                table: "GarmentSewingDOs");

            migrationBuilder.DropIndex(
                name: "IX_GarmentLoadings_LoadingNo",
                table: "GarmentLoadings");

            migrationBuilder.DropIndex(
                name: "IX_GarmentFinishingIns_FinishingInNo",
                table: "GarmentFinishingIns");

            migrationBuilder.DropIndex(
                name: "IX_GarmentDeliveryReturns_DRNo",
                table: "GarmentDeliveryReturns");

            migrationBuilder.DropIndex(
                name: "IX_GarmentCuttingOuts_CutOutNo",
                table: "GarmentCuttingOuts");
        }
    }
}
