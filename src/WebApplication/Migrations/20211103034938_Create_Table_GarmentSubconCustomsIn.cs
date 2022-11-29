using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Create_Table_GarmentSubconCustomsIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCustomsIns",
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
                    BcNo = table.Column<string>(maxLength: 255, nullable: true),
                    BcDate = table.Column<DateTimeOffset>(nullable: false),
                    BcType = table.Column<string>(maxLength: 255, nullable: true),
                    SubconType = table.Column<string>(maxLength: 255, nullable: true),
                    SubconContractId = table.Column<Guid>(nullable: false),
                    SubconContractNo = table.Column<string>(maxLength: 100, nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 25, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCustomsIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconCustomsInItems",
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
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 25, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 100, nullable: true),
                    DoId = table.Column<int>(nullable: false),
                    DoNo = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    SubconCustomsInId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconCustomsInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconCustomsInItems_GarmentSubconCustomsIns_SubconCustomsInId",
                        column: x => x.SubconCustomsInId,
                        principalTable: "GarmentSubconCustomsIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconCustomsInItems_SubconCustomsInId",
                table: "GarmentServiceSubconCustomsInItems",
                column: "SubconCustomsInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCustomsIns_BcNo",
                table: "GarmentSubconCustomsIns",
                column: "BcNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconCustomsInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCustomsIns");
        }
    }
}
