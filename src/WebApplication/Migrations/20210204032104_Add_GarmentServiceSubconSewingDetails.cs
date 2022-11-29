using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_GarmentServiceSubconSewingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Article",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "ComodityName",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "IsDifferentSize",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "RONo",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "DesignColor",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "RemainingQuantity",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "SewingInId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "SewingInItemId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.RenameColumn(
                name: "UomUnit",
                table: "GarmentServiceSubconSewingItems",
                newName: "RONo");

            migrationBuilder.RenameColumn(
                name: "UomId",
                table: "GarmentServiceSubconSewingItems",
                newName: "ComodityId");

            migrationBuilder.RenameColumn(
                name: "SizeName",
                table: "GarmentServiceSubconSewingItems",
                newName: "ComodityName");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "GarmentServiceSubconSewingItems",
                newName: "ComodityCode");

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "GarmentServiceSubconSewingItems",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconSewingDetails",
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
                    ServiceSubconSewingItemId = table.Column<Guid>(nullable: false),
                    SewingInId = table.Column<Guid>(nullable: false),
                    SewingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconSewingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconSewingDetails_GarmentServiceSubconSewingItems_ServiceSubconSewingItemId",
                        column: x => x.ServiceSubconSewingItemId,
                        principalTable: "GarmentServiceSubconSewingItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconSewingDetails_ServiceSubconSewingItemId",
                table: "GarmentServiceSubconSewingDetails",
                column: "ServiceSubconSewingItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconSewingDetails");

            migrationBuilder.DropColumn(
                name: "Article",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.RenameColumn(
                name: "RONo",
                table: "GarmentServiceSubconSewingItems",
                newName: "UomUnit");

            migrationBuilder.RenameColumn(
                name: "ComodityName",
                table: "GarmentServiceSubconSewingItems",
                newName: "SizeName");

            migrationBuilder.RenameColumn(
                name: "ComodityId",
                table: "GarmentServiceSubconSewingItems",
                newName: "UomId");

            migrationBuilder.RenameColumn(
                name: "ComodityCode",
                table: "GarmentServiceSubconSewingItems",
                newName: "ProductCode");

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "GarmentServiceSubconSewings",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentServiceSubconSewings",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComodityName",
                table: "GarmentServiceSubconSewings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDifferentSize",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RONo",
                table: "GarmentServiceSubconSewings",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentServiceSubconSewings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "GarmentServiceSubconSewingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignColor",
                table: "GarmentServiceSubconSewingItems",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentServiceSubconSewingItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RemainingQuantity",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "SewingInId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewingInItemId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentServiceSubconSewingItems",
                nullable: true);
        }
    }
}
