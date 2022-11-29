using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Feature_GarmentServiceSubconShrinkagePanel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconShrinkagePanels",
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
                    ServiceSubconShrinkagePanelNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSubconShrinkagePanelDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconShrinkagePanels", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconShrinkagePanelItems",
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
                    ServiceSubconShrinkagePanelId = table.Column<Guid>(nullable: false),
                    UnitExpenditureNo = table.Column<string>(maxLength: 25, nullable: true),
                    ExpenditureDate = table.Column<DateTimeOffset>(nullable: false),
                    UnitSenderId = table.Column<int>(nullable: false),
                    UnitSenderCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitSenderName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitRequestId = table.Column<int>(nullable: false),
                    UnitRequestCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitRequestName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconShrinkagePanelItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconShrinkagePanelItems_GarmentServiceSubconShrinkagePanels_ServiceSubconShrinkagePanelId",
                        column: x => x.ServiceSubconShrinkagePanelId,
                        principalTable: "GarmentServiceSubconShrinkagePanels",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconShrinkagePanelDetails",
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
                    ServiceSubconShrinkagePanelItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 1000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconShrinkagePanelDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconShrinkagePanelDetails_GarmentServiceSubconShrinkagePanelItems_ServiceSubconShrinkagePanelItemId",
                        column: x => x.ServiceSubconShrinkagePanelItemId,
                        principalTable: "GarmentServiceSubconShrinkagePanelItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconShrinkagePanelDetails_ServiceSubconShrinkagePanelItemId",
                table: "GarmentServiceSubconShrinkagePanelDetails",
                column: "ServiceSubconShrinkagePanelItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconShrinkagePanelItems_ServiceSubconShrinkagePanelId",
                table: "GarmentServiceSubconShrinkagePanelItems",
                column: "ServiceSubconShrinkagePanelId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconShrinkagePanels_ServiceSubconShrinkagePanelNo",
                table: "GarmentServiceSubconShrinkagePanels",
                column: "ServiceSubconShrinkagePanelNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconShrinkagePanelDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSubconShrinkagePanelItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSubconShrinkagePanels");
        }
    }
}
