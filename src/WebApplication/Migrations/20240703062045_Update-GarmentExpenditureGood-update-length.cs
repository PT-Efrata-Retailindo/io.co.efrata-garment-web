using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class UpdateGarmentExpenditureGoodupdatelength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentExpenditureGoods",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentExpenditureGoods",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
