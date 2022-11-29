using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Tables_For_GarmentAvalComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentAvalComponents",
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
                    AvalComponentNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<long>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    AvalComponentType = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<long>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentAvalComponents", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentAvalComponentItems",
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
                    AvalComponentId = table.Column<Guid>(nullable: false),
                    CuttingInDetailId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 100, nullable: true),
                    Color = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    SizeId = table.Column<long>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentAvalComponentItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentAvalComponentItems_GarmentAvalComponents_AvalComponentId",
                        column: x => x.AvalComponentId,
                        principalTable: "GarmentAvalComponents",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentAvalComponentItems_AvalComponentId",
                table: "GarmentAvalComponentItems",
                column: "AvalComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentAvalComponents_AvalComponentNo",
                table: "GarmentAvalComponents",
                column: "AvalComponentNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentAvalComponentItems");

            migrationBuilder.DropTable(
                name: "GarmentAvalComponents");
        }
    }
}
