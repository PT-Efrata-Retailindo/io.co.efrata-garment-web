using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Create_Table_GarmentSampleCuttingIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSampleCuttingIns",
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
                    CutInNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingType = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingInDate = table.Column<DateTimeOffset>(nullable: false),
                    FC = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleCuttingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleCuttingInItems",
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
                    CutInId = table.Column<Guid>(nullable: false),
                    PreparingId = table.Column<Guid>(nullable: false),
                    SewingOutId = table.Column<Guid>(nullable: false),
                    SewingOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleCuttingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleCuttingInItems_GarmentSampleCuttingIns_CutInId",
                        column: x => x.CutInId,
                        principalTable: "GarmentSampleCuttingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleCuttingInDetails",
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
                    CutInItemId = table.Column<Guid>(nullable: false),
                    PreparingItemId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    FabricType = table.Column<string>(maxLength: 25, nullable: true),
                    PreparingQuantity = table.Column<double>(nullable: false),
                    PreparingUomId = table.Column<int>(nullable: false),
                    PreparingUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    CuttingInQuantity = table.Column<int>(nullable: false),
                    CuttingInUomId = table.Column<int>(nullable: false),
                    CuttingInUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    Color = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleCuttingInDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleCuttingInDetails_GarmentSampleCuttingInItems_CutInItemId",
                        column: x => x.CutInItemId,
                        principalTable: "GarmentSampleCuttingInItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleCuttingInDetails_CutInItemId",
                table: "GarmentSampleCuttingInDetails",
                column: "CutInItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleCuttingInItems_CutInId",
                table: "GarmentSampleCuttingInItems",
                column: "CutInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleCuttingIns_CutInNo",
                table: "GarmentSampleCuttingIns",
                column: "CutInNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSampleCuttingInDetails");

            migrationBuilder.DropTable(
                name: "GarmentSampleCuttingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleCuttingIns");
        }
    }
}
