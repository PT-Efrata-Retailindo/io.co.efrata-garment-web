using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class initial_GarmentSampleRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSampleRequests",
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
                    SampleCategory = table.Column<string>(maxLength: 50, nullable: true),
                    SampleRequestNo = table.Column<string>(maxLength: 30, nullable: true),
                    RONoSample = table.Column<string>(maxLength: 15, nullable: true),
                    RONoCC = table.Column<string>(maxLength: 15, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 255, nullable: true),
                    SampleType = table.Column<string>(maxLength: 255, nullable: true),
                    Packing = table.Column<string>(maxLength: 255, nullable: true),
                    SentDate = table.Column<DateTimeOffset>(nullable: false),
                    POBuyer = table.Column<string>(maxLength: 255, nullable: true),
                    Attached = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true),
                    IsPosted = table.Column<bool>(nullable: false),
                    IsReceived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleRequests", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleRequestProducts",
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
                    SampleRequestId = table.Column<Guid>(nullable: false),
                    Style = table.Column<string>(maxLength: 255, nullable: true),
                    Color = table.Column<string>(maxLength: 500, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    SizeDescription = table.Column<string>(maxLength: 500, nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleRequestProducts", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleRequestProducts_GarmentSampleRequests_SampleRequestId",
                        column: x => x.SampleRequestId,
                        principalTable: "GarmentSampleRequests",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleRequestSpecifications",
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
                    SampleRequestId = table.Column<Guid>(nullable: false),
                    Inventory = table.Column<string>(maxLength: 25, nullable: true),
                    SpecificationDetail = table.Column<string>(maxLength: 500, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleRequestSpecifications", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleRequestSpecifications_GarmentSampleRequests_SampleRequestId",
                        column: x => x.SampleRequestId,
                        principalTable: "GarmentSampleRequests",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleRequestProducts_SampleRequestId",
                table: "GarmentSampleRequestProducts",
                column: "SampleRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleRequests_SampleRequestNo",
                table: "GarmentSampleRequests",
                column: "SampleRequestNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleRequestSpecifications_SampleRequestId",
                table: "GarmentSampleRequestSpecifications",
                column: "SampleRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSampleRequestProducts");

            migrationBuilder.DropTable(
                name: "GarmentSampleRequestSpecifications");

            migrationBuilder.DropTable(
                name: "GarmentSampleRequests");
        }
    }
}
