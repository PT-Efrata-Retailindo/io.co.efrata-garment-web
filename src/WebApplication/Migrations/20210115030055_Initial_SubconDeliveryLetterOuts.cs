using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Initial_SubconDeliveryLetterOuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconDeliveryLetterOuts",
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
                    DLNo = table.Column<string>(maxLength: 25, nullable: true),
                    DLType = table.Column<string>(nullable: true),
                    SubconContractId = table.Column<Guid>(nullable: false),
                    ContractNo = table.Column<string>(maxLength: 25, nullable: true),
                    ContractType = table.Column<string>(maxLength: 25, nullable: true),
                    DLDate = table.Column<DateTimeOffset>(nullable: false),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 25, nullable: true),
                    PONo = table.Column<string>(maxLength: 25, nullable: true),
                    EPOItemId = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconDeliveryLetterOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconDeliveryLetterOutItems",
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
                    SubconDeliveryLetterOutId = table.Column<Guid>(nullable: false),
                    UENItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 2000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    FabricType = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconDeliveryLetterOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_SubconDeliveryLetterOutId",
                        column: x => x.SubconDeliveryLetterOutId,
                        principalTable: "GarmentSubconDeliveryLetterOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconDeliveryLetterOuts_DLNo",
                table: "GarmentSubconDeliveryLetterOuts",
                column: "DLNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems",
                column: "SubconDeliveryLetterOutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconDeliveryLetterOuts");
        }
    }
}
