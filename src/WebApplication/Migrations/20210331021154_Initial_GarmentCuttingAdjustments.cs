using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initial_GarmentCuttingAdjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentCuttingAdjustments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    AdjustmentNo = table.Column<string>(maxLength: 25, nullable: true),
                    CutInNo = table.Column<string>(maxLength: 25, nullable: true),
                    CutInId = table.Column<Guid>(nullable: false),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    TotalFC = table.Column<decimal>(nullable: false),
                    TotalActualFC = table.Column<decimal>(nullable: false),
                    TotalQuantity = table.Column<decimal>(nullable: false),
                    TotalActualQuantity = table.Column<decimal>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    AdjustmentDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentCuttingAdjustments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentCuttingAdjustmentItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    AdjustmentCuttingId = table.Column<Guid>(nullable: false),
                    CutInDetailId = table.Column<Guid>(nullable: false),
                    PreparingItemId = table.Column<Guid>(nullable: false),
                    FC = table.Column<decimal>(nullable: false),
                    ActualFC = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    ActualQuantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentCuttingAdjustmentItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentCuttingAdjustmentItems_GarmentCuttingAdjustments_AdjustmentCuttingId",
                        column: x => x.AdjustmentCuttingId,
                        principalTable: "GarmentCuttingAdjustments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentCuttingAdjustmentItems_AdjustmentCuttingId",
                table: "GarmentCuttingAdjustmentItems",
                column: "AdjustmentCuttingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentCuttingAdjustments_AdjustmentNo",
                table: "GarmentCuttingAdjustments",
                column: "AdjustmentNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentCuttingAdjustmentItems");

            migrationBuilder.DropTable(
                name: "GarmentCuttingAdjustments");
        }
    }
}
