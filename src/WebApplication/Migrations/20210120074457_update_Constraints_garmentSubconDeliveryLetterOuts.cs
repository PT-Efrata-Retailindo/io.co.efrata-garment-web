using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_Constraints_garmentSubconDeliveryLetterOuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "UENItemId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.AddColumn<Guid>(
                name: "GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems",
                column: "GarmentSubconDeliveryLetterOutIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems",
                column: "GarmentSubconDeliveryLetterOutIdentity",
                principalTable: "GarmentSubconDeliveryLetterOuts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.AddColumn<Guid>(
                name: "UENItemId",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems",
                column: "SubconDeliveryLetterOutId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems",
                column: "SubconDeliveryLetterOutId",
                principalTable: "GarmentSubconDeliveryLetterOuts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
