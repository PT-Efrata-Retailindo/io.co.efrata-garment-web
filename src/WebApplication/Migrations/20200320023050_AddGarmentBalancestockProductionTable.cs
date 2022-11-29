using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddGarmentBalancestockProductionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentBalanceStockProductions",
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
                    RO = table.Column<string>(maxLength: 25, nullable: true),
                    ArticleNo = table.Column<string>(maxLength: 100, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    QtyOrder = table.Column<double>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    Wage = table.Column<decimal>(nullable: false),
                    OTL = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentBalanceStockProductions", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentBalanceStockProductions");
        }
    }
}
