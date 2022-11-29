using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class change_GarmentServiceSubconCuttings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Article",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "ComodityName",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "RONo",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComodityName",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "RONo",
                table: "GarmentServiceSubconCuttingItems",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Article",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "ComodityName",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.DropColumn(
                name: "RONo",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "GarmentServiceSubconCuttings",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentServiceSubconCuttings",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentServiceSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComodityName",
                table: "GarmentServiceSubconCuttings",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RONo",
                table: "GarmentServiceSubconCuttings",
                maxLength: 25,
                nullable: true);
        }
    }
}
