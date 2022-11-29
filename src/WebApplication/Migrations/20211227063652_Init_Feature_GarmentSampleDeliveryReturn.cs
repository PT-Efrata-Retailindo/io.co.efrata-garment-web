using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Init_Feature_GarmentSampleDeliveryReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSampleDeliveryReturns",
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
                    DRNo = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 100, nullable: true),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    UnitDOId = table.Column<int>(nullable: false),
                    UnitDONo = table.Column<string>(maxLength: 100, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    PreparingId = table.Column<string>(nullable: true),
                    ReturnDate = table.Column<DateTimeOffset>(nullable: true),
                    ReturnType = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    StorageId = table.Column<int>(nullable: false),
                    StorageCode = table.Column<string>(maxLength: 25, nullable: true),
                    StorageName = table.Column<string>(maxLength: 100, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleDeliveryReturns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleDeliveryReturnItems",
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
                    DRId = table.Column<Guid>(nullable: false),
                    UnitDOItemId = table.Column<int>(nullable: false),
                    UENItemId = table.Column<int>(nullable: false),
                    PreparingItemId = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    RONo = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleDeliveryReturnItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleDeliveryReturnItems_GarmentSampleDeliveryReturns_DRId",
                        column: x => x.DRId,
                        principalTable: "GarmentSampleDeliveryReturns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleDeliveryReturnItems_DRId",
                table: "GarmentSampleDeliveryReturnItems",
                column: "DRId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleDeliveryReturns_DRNo",
                table: "GarmentSampleDeliveryReturns",
                column: "DRNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleDeliveryReturns");
        }
    }
}
