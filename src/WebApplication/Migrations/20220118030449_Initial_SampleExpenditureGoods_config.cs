using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initial_SampleExpenditureGoods_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSampleExpenditureGoods",
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
                    ExpenditureGoodNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    ExpenditureType = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 100, nullable: true),
                    ExpenditureDate = table.Column<DateTimeOffset>(nullable: false),
                    Invoice = table.Column<string>(maxLength: 50, nullable: true),
                    PackingListId = table.Column<int>(nullable: false),
                    ContractNo = table.Column<string>(nullable: true),
                    Carton = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsReceived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleExpenditureGoods", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleExpenditureGoodItems",
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
                    ExpenditureGoodId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReturQuantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleExpenditureGoodItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleExpenditureGoodItems_GarmentSampleExpenditureGoods_ExpenditureGoodId",
                        column: x => x.ExpenditureGoodId,
                        principalTable: "GarmentSampleExpenditureGoods",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleExpenditureGoodItems_ExpenditureGoodId",
                table: "GarmentSampleExpenditureGoodItems",
                column: "ExpenditureGoodId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleExpenditureGoods_ExpenditureGoodNo",
                table: "GarmentSampleExpenditureGoods",
                column: "ExpenditureGoodNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSampleExpenditureGoodItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleExpenditureGoods");
        }
    }
}
