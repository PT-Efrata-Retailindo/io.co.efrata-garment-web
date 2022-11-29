using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Remove_table_GarmentBalanceStockProduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentBalanceStockProductions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentBalanceStockProductions",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    ArticleNo = table.Column<string>(maxLength: 100, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    FC = table.Column<double>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    OTL = table.Column<decimal>(nullable: false),
                    Periode = table.Column<DateTimeOffset>(nullable: false),
                    QtyOrder = table.Column<double>(nullable: false),
                    RO = table.Column<string>(maxLength: 25, nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Wage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentBalanceStockProductions", x => x.Identity);
                });
        }
    }
}
