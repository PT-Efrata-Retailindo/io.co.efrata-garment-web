using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddField_ReceivedDate_ReceivedBy_GarmentSampleRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "GarmentSampleRequests",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReceivedDate",
                table: "GarmentSampleRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "GarmentSampleRequests");
        }
    }
}
