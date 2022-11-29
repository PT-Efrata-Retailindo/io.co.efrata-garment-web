using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_CuttingInDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CuttingFrom",
                table: "GarmentCuttingIns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SewingOutNo",
                table: "GarmentCuttingInItems",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutDetailId",
                table: "GarmentCuttingInDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewingOutItemId",
                table: "GarmentCuttingInDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SewingOutDetailId",
                table: "GarmentCuttingInDetails");

            migrationBuilder.DropColumn(
                name: "SewingOutItemId",
                table: "GarmentCuttingInDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CuttingFrom",
                table: "GarmentCuttingIns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SewingOutNo",
                table: "GarmentCuttingInItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
