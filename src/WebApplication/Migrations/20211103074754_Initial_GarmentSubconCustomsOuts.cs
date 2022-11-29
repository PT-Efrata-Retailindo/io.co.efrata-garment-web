using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initial_GarmentSubconCustomsOuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCustomsOuts",
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
                    CustomsOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    CustomsOutDate = table.Column<DateTimeOffset>(nullable: false),
                    CustomsOutType = table.Column<string>(maxLength: 50, nullable: true),
                    SubconType = table.Column<string>(maxLength: 50, nullable: true),
                    SubconContractId = table.Column<Guid>(nullable: false),
                    SubconContractNo = table.Column<string>(maxLength: 25, nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 25, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCustomsOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCustomsOutItems",
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
                    SubconDLOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    SubconDLOutId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    SubconCustomsOutId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCustomsOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCustomsOutItems_GarmentSubconCustomsOuts_SubconCustomsOutId",
                        column: x => x.SubconCustomsOutId,
                        principalTable: "GarmentSubconCustomsOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCustomsOutItems_SubconCustomsOutId",
                table: "GarmentSubconCustomsOutItems",
                column: "SubconCustomsOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCustomsOuts_CustomsOutNo",
                table: "GarmentSubconCustomsOuts",
                column: "CustomsOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconCustomsOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCustomsOuts");
        }
    }
}
