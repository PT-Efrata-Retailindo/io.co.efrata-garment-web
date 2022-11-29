using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_GarmentServiceSubconCuttingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "DesignColor",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconCuttingDetails",
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
                    CuttingInDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ServiceSubconCuttingItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconCuttingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconCuttingDetails_GarmentServiceSubconCuttingItems_ServiceSubconCuttingItemId",
                        column: x => x.ServiceSubconCuttingItemId,
                        principalTable: "GarmentServiceSubconCuttingItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconCuttingDetails_ServiceSubconCuttingItemId",
                table: "GarmentServiceSubconCuttingDetails",
                column: "ServiceSubconCuttingItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DesignColor",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
