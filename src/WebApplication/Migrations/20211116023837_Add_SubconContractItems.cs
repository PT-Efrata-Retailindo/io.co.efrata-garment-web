using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_SubconContractItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentServiceSubconCustomsInItems_GarmentSubconCustomsIns_SubconCustomsInId",
                table: "GarmentServiceSubconCustomsInItems");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "GarmentSubconContracts",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuyerName",
                table: "GarmentSubconContracts",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuyerCode",
                table: "GarmentSubconContracts",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AgreementDate",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "SKEPNo",
                table: "GarmentSubconContracts",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubconCategory",
                table: "GarmentSubconContracts",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UomId",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentSubconContracts",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GarmentSubconContractItems",
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
                    SubconContractId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconContractItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconContractItems_GarmentSubconContracts_SubconContractId",
                        column: x => x.SubconContractId,
                        principalTable: "GarmentSubconContracts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconContractItems_SubconContractId",
                table: "GarmentSubconContractItems",
                column: "SubconContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentServiceSubconCustomsInItems_GarmentSubconCustomsIns_SubconCustomsInId",
                table: "GarmentServiceSubconCustomsInItems",
                column: "SubconCustomsInId",
                principalTable: "GarmentSubconCustomsIns",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentServiceSubconCustomsInItems_GarmentSubconCustomsIns_SubconCustomsInId",
                table: "GarmentServiceSubconCustomsInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconContractItems");

            migrationBuilder.DropColumn(
                name: "AgreementDate",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "SKEPNo",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "SubconCategory",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentSubconContracts");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "GarmentSubconContracts",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuyerName",
                table: "GarmentSubconContracts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuyerCode",
                table: "GarmentSubconContracts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentServiceSubconCustomsInItems_GarmentSubconCustomsIns_SubconCustomsInId",
                table: "GarmentServiceSubconCustomsInItems",
                column: "SubconCustomsInId",
                principalTable: "GarmentSubconCustomsIns",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
