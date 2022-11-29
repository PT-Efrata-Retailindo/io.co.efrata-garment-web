using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addbalanceProductionSstockTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentBalanceProductionStocks",
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
                    Ro = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 50, nullable: true),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    Comodity = table.Column<string>(maxLength: 50, nullable: true),
                    QtyOrder = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Fare = table.Column<decimal>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    BeginingBalanceCuttingQty = table.Column<double>(nullable: false),
                    BeginingBalanceCuttingPrice = table.Column<double>(nullable: false),
                    BeginingBalanceLoadingQty = table.Column<double>(nullable: false),
                    BeginingBalanceLoadingPrice = table.Column<double>(nullable: false),
                    BeginingBalanceSewingQty = table.Column<double>(nullable: false),
                    BeginingBalanceSewingPrice = table.Column<double>(nullable: false),
                    BeginingBalanceFinishingQty = table.Column<double>(nullable: false),
                    BeginingBalanceFinishingPrice = table.Column<double>(nullable: false),
                    BeginingBalanceSubconQty = table.Column<double>(nullable: false),
                    BeginingBalanceSubconPrice = table.Column<double>(nullable: false),
                    BeginingBalanceExpenditureGood = table.Column<double>(nullable: false),
                    BeginingBalanceExpenditureGoodPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentBalanceProductionStocks", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentBalanceProductionStocks");
        }
    }
}
