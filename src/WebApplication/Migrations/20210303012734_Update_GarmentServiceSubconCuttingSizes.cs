using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_GarmentServiceSubconCuttingSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.DropColumn(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.AlterColumn<string>(
                name: "UomOutUnit",
                table: "GarmentSubconDeliveryLetterOutItems",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingSizes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingSizes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingSizes",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingSizes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingSizes",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingSizes");

            migrationBuilder.DropColumn(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingSizes");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingSizes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingSizes");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingSizes");

            migrationBuilder.AlterColumn<string>(
                name: "UomOutUnit",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInDetailId",
                table: "GarmentServiceSubconCuttingDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentServiceSubconCuttingDetails",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GarmentServiceSubconCuttingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentServiceSubconCuttingDetails",
                maxLength: 100,
                nullable: true);
        }
    }
}
