using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_garmentSampleRequests_Add_Columns_Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "GarmentSampleRequestSpecifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsFileName",
                table: "GarmentSampleRequests",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsPath",
                table: "GarmentSampleRequests",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagesName",
                table: "GarmentSampleRequests",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagesPath",
                table: "GarmentSampleRequests",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "GarmentSampleRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevised",
                table: "GarmentSampleRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectedBy",
                table: "GarmentSampleRequests",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RejectedDate",
                table: "GarmentSampleRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisedBy",
                table: "GarmentSampleRequests",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisedReason",
                table: "GarmentSampleRequests",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "GarmentSampleRequestProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "GarmentSampleRequestSpecifications");

            migrationBuilder.DropColumn(
                name: "DocumentsFileName",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "DocumentsPath",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "ImagesName",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "ImagesPath",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "IsRevised",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "RejectedBy",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "RejectedDate",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "RevisedBy",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "RevisedReason",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "GarmentSampleRequestProducts");
        }
    }
}
