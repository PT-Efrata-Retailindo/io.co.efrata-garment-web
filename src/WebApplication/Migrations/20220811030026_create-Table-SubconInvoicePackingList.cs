using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class createTableSubconInvoicePackingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubconInvoicePackingList",
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
                    InvoiceNo = table.Column<string>(maxLength: 50, nullable: true),
                    BCType = table.Column<string>(maxLength: 25, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierAddress = table.Column<string>(maxLength: 255, nullable: true),
                    ContractNo = table.Column<string>(maxLength: 50, nullable: true),
                    NW = table.Column<double>(nullable: false),
                    GW = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubconInvoicePackingList", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "SubconInvoicePackingListItems",
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
                    InvoicePackingListId = table.Column<Guid>(nullable: false),
                    DLNo = table.Column<string>(maxLength: 50, nullable: true),
                    DLDate = table.Column<DateTimeOffset>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 255, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    CIF = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubconInvoicePackingListItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_SubconInvoicePackingListItems_SubconInvoicePackingList_InvoicePackingListId",
                        column: x => x.InvoicePackingListId,
                        principalTable: "SubconInvoicePackingList",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubconInvoicePackingList_InvoiceNo",
                table: "SubconInvoicePackingList",
                column: "InvoiceNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_SubconInvoicePackingListItems_InvoicePackingListId",
                table: "SubconInvoicePackingListItems",
                column: "InvoicePackingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubconInvoicePackingListItems");

            migrationBuilder.DropTable(
                name: "SubconInvoicePackingList");
        }
    }
}
