using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class initial_GarmentSampleSewingIns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSampleCuttingOutDetails_GarmentSampleCuttingOutItems_CutOutItemId",
                table: "GarmentSampleCuttingOutDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSampleCuttingOutItems_GarmentSampleCuttingOuts_CutOutId",
                table: "GarmentSampleCuttingOutItems");

            migrationBuilder.RenameColumn(
                name: "CutOutId",
                table: "GarmentSampleCuttingOutItems",
                newName: "CuttingOutId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSampleCuttingOutItems_CutOutId",
                table: "GarmentSampleCuttingOutItems",
                newName: "IX_GarmentSampleCuttingOutItems_CuttingOutId");

            migrationBuilder.RenameColumn(
                name: "CutOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                newName: "CuttingOutItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSampleCuttingOutDetails_CutOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                newName: "IX_GarmentSampleCuttingOutDetails_CuttingOutItemId");

            migrationBuilder.CreateTable(
                name: "GarmentSampleSewingIns",
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
                    SewingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    SewingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingOutId = table.Column<Guid>(nullable: false),
                    CuttingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingInDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleSewingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleSewingInItems",
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
                    SewingInId = table.Column<Guid>(nullable: false),
                    CuttingOutItemId = table.Column<Guid>(nullable: false),
                    CuttingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleSewingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleSewingInItems_GarmentSampleSewingIns_SewingInId",
                        column: x => x.SewingInId,
                        principalTable: "GarmentSampleSewingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleSewingInItems_SewingInId",
                table: "GarmentSampleSewingInItems",
                column: "SewingInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleSewingIns_SewingInNo",
                table: "GarmentSampleSewingIns",
                column: "SewingInNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSampleCuttingOutDetails_GarmentSampleCuttingOutItems_CuttingOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                column: "CuttingOutItemId",
                principalTable: "GarmentSampleCuttingOutItems",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSampleCuttingOutItems_GarmentSampleCuttingOuts_CuttingOutId",
                table: "GarmentSampleCuttingOutItems",
                column: "CuttingOutId",
                principalTable: "GarmentSampleCuttingOuts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSampleCuttingOutDetails_GarmentSampleCuttingOutItems_CuttingOutItemId",
                table: "GarmentSampleCuttingOutDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSampleCuttingOutItems_GarmentSampleCuttingOuts_CuttingOutId",
                table: "GarmentSampleCuttingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleSewingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleSewingIns");

            migrationBuilder.RenameColumn(
                name: "CuttingOutId",
                table: "GarmentSampleCuttingOutItems",
                newName: "CutOutId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSampleCuttingOutItems_CuttingOutId",
                table: "GarmentSampleCuttingOutItems",
                newName: "IX_GarmentSampleCuttingOutItems_CutOutId");

            migrationBuilder.RenameColumn(
                name: "CuttingOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                newName: "CutOutItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSampleCuttingOutDetails_CuttingOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                newName: "IX_GarmentSampleCuttingOutDetails_CutOutItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSampleCuttingOutDetails_GarmentSampleCuttingOutItems_CutOutItemId",
                table: "GarmentSampleCuttingOutDetails",
                column: "CutOutItemId",
                principalTable: "GarmentSampleCuttingOutItems",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSampleCuttingOutItems_GarmentSampleCuttingOuts_CutOutId",
                table: "GarmentSampleCuttingOutItems",
                column: "CutOutId",
                principalTable: "GarmentSampleCuttingOuts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
