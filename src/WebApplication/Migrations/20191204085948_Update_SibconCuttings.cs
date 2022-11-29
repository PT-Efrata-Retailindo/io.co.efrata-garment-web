using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_SibconCuttings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComodityName",
                table: "GarmentSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignColor",
                table: "GarmentSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "GarmentSubconCuttings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ComodityName",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "DesignColor",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentSubconCuttings");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "GarmentSubconCuttings");
        }
    }
}
