using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class replaceDOItemIdColumnWithDODetailId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOItemId",
                table: "GarmentFinishingInItems",
                newName: "DODetailId");

            migrationBuilder.AlterColumn<string>(
                name: "DONo",
                table: "GarmentFinishingIns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DODetailId",
                table: "GarmentFinishingInItems",
                newName: "DOItemId");

            migrationBuilder.AlterColumn<string>(
                name: "DONo",
                table: "GarmentFinishingIns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
