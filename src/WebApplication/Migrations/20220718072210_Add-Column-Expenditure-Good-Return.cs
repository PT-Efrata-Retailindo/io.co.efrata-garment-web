using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddColumnExpenditureGoodReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BCNo",
                table: "GarmentExpenditureGoodReturns",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCType",
                table: "GarmentExpenditureGoodReturns",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DONo",
                table: "GarmentExpenditureGoodReturns",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpenditureNo",
                table: "GarmentExpenditureGoodReturns",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URNNo",
                table: "GarmentExpenditureGoodReturns",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BCNo",
                table: "GarmentExpenditureGoodReturns");

            migrationBuilder.DropColumn(
                name: "BCType",
                table: "GarmentExpenditureGoodReturns");

            migrationBuilder.DropColumn(
                name: "DONo",
                table: "GarmentExpenditureGoodReturns");

            migrationBuilder.DropColumn(
                name: "ExpenditureNo",
                table: "GarmentExpenditureGoodReturns");

            migrationBuilder.DropColumn(
                name: "URNNo",
                table: "GarmentExpenditureGoodReturns");
        }
    }
}
