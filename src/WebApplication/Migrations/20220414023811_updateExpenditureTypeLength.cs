using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updateExpenditureTypeLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExpenditureType",
                table: "GarmentSampleExpenditureGoods",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExpenditureType",
                table: "GarmentSampleExpenditureGoods",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 75,
                oldNullable: true);
        }
    }
}
