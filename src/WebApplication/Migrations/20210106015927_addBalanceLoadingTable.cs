using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addBalanceLoadingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentBalanceLoadings",
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
                    RoJob = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 50, nullable: true),
                    QtyOrder = table.Column<double>(nullable: false),
                    Style = table.Column<string>(maxLength: 50, nullable: true),
                    Hours = table.Column<double>(nullable: false),
                    Stock = table.Column<double>(nullable: false),
                    CuttingQtyPcs = table.Column<double>(nullable: false),
                    LoadingQtyPcs = table.Column<double>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    RemainQty = table.Column<double>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Nominal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentBalanceLoadings", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentBalanceLoadings");
        }
    }
}
