using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_CuttingIn_Item_Detail_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GarmentCuttingInItems_CutInId",
                table: "GarmentCuttingInItems",
                column: "CutInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentCuttingInDetails_CutInItemId",
                table: "GarmentCuttingInDetails",
                column: "CutInItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentCuttingInDetails_GarmentCuttingInItems_CutInItemId",
                table: "GarmentCuttingInDetails",
                column: "CutInItemId",
                principalTable: "GarmentCuttingInItems",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentCuttingInItems_GarmentCuttingIns_CutInId",
                table: "GarmentCuttingInItems",
                column: "CutInId",
                principalTable: "GarmentCuttingIns",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentCuttingInDetails_GarmentCuttingInItems_CutInItemId",
                table: "GarmentCuttingInDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentCuttingInItems_GarmentCuttingIns_CutInId",
                table: "GarmentCuttingInItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentCuttingInItems_CutInId",
                table: "GarmentCuttingInItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentCuttingInDetails_CutInItemId",
                table: "GarmentCuttingInDetails");
        }
    }
}
