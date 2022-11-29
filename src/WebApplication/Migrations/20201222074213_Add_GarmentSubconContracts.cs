using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_GarmentSubconContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconContracts",
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
                    ContractType = table.Column<string>(maxLength: 50, nullable: true),
                    ContractNo = table.Column<string>(maxLength: 50, nullable: true),
                    AgreementNo = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 25, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 100, nullable: true),
                    JobType = table.Column<string>(maxLength: 50, nullable: true),
                    BPJNo = table.Column<string>(maxLength: 50, nullable: true),
                    FinishedGoodType = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    DueDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconContracts", x => x.Identity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconContracts_ContractNo",
                table: "GarmentSubconContracts",
                column: "ContractNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconContracts");
        }
    }
}
