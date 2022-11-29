using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addBalanceCuttingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentBalanceCuttings",
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
                    Id = table.Column<Guid>(nullable: false),
                    RoJob = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 50, nullable: true),
                    QtyOrder = table.Column<double>(nullable: false),
                    Style = table.Column<string>(maxLength: 50, nullable: true),
                    Hours = table.Column<double>(nullable: false),
                    Stock = table.Column<double>(nullable: false),
                    CuttingQtyMeter = table.Column<double>(nullable: false),
                    CuttingQtyPcs = table.Column<double>(nullable: false),
                    Fc = table.Column<double>(nullable: false),
                    Expenditure = table.Column<double>(nullable: false),
                    RemainQty = table.Column<double>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Nominal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentBalanceCuttings", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentBalanceCuttings");
        }
    }
}
