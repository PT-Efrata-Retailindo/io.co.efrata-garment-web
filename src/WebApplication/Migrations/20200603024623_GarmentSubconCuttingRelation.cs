using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class GarmentSubconCuttingRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingRelations",
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
                    GarmentSubconCuttingId = table.Column<Guid>(nullable: false),
                    GarmentCuttingOutId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingRelations", x => x.Identity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId",
                table: "GarmentSubconCuttingRelations",
                column: "GarmentCuttingOutId",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingRelations_GarmentCuttingOutId_GarmentSubconCuttingId",
                table: "GarmentSubconCuttingRelations",
                columns: new[] { "GarmentCuttingOutId", "GarmentSubconCuttingId" },
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingRelations");
        }
    }
}
