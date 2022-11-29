using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Feature_GarmentServiceSubconFabricWash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconFabricWashes",
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
                    ServiceSubconFabricWashNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSubconFabricWashDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconFabricWashes", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconFabricWashItems",
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
                    ServiceSubconFabricWashId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_GarmentServiceSubconFabricWashItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconFabricWashItems_GarmentServiceSubconFabricWashes_ServiceSubconFabricWashId",
                        column: x => x.ServiceSubconFabricWashId,
                        principalTable: "GarmentServiceSubconFabricWashes",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconFabricWashDetails",
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
                    ServiceSubconFabricWashItemId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_GarmentServiceSubconFabricWashDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconFabricWashDetails_GarmentServiceSubconFabricWashItems_ServiceSubconFabricWashItemId",
                        column: x => x.ServiceSubconFabricWashItemId,
                        principalTable: "GarmentServiceSubconFabricWashItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconFabricWashDetails_ServiceSubconFabricWashItemId",
                table: "GarmentServiceSubconFabricWashDetails",
                column: "ServiceSubconFabricWashItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconFabricWashItems_ServiceSubconFabricWashId",
                table: "GarmentServiceSubconFabricWashItems",
                column: "ServiceSubconFabricWashId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconFabricWashes_ServiceSubconFabricWashNo",
                table: "GarmentServiceSubconFabricWashes",
                column: "ServiceSubconFabricWashNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconFabricWashDetails");

            migrationBuilder.DropTable(
                name: "GarmentServiceSubconFabricWashItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSubconFabricWashes");

        }
    }
}
