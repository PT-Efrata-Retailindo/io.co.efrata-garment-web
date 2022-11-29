using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class GarmentSubconCuttingRelation_RenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GarmentCuttingOutId",
                table: "GarmentSubconCuttingRelations",
                newName: "GarmentCuttingOutDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId_GarmentSubconCuttingId",
                table: "GarmentSubconCuttingRelations",
                newName: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutDetailId_GarmentSubconCuttingId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId",
                table: "GarmentSubconCuttingRelations",
                newName: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GarmentCuttingOutDetailId",
                table: "GarmentSubconCuttingRelations",
                newName: "GarmentCuttingOutId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutDetailId_GarmentSubconCuttingId",
                table: "GarmentSubconCuttingRelations",
                newName: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId_GarmentSubconCuttingId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutDetailId",
                table: "GarmentSubconCuttingRelations",
                newName: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId");
        }
    }
}
