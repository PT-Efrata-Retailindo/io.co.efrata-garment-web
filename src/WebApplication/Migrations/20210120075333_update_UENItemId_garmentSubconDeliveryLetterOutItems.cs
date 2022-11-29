using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_UENItemId_garmentSubconDeliveryLetterOutItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentSubconDeliveryLetterOuts",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.DropColumn(
                name: "GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.RenameTable(
                name: "GarmentSubconDeliveryLetterOuts",
                newName: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconDeliveryLetterOuts_DLNo",
                table: "GarmentSubconDeliveryLetterOuts",
                newName: "IX_GarmentSubconDeliveryLetterOuts_DLNo");

            migrationBuilder.AddColumn<int>(
                name: "UENItemId",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentSubconDeliveryLetterOuts",
                table: "GarmentSubconDeliveryLetterOuts",
                column: "Identity");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconDeliveryLetterOutItems_GarmentSubconDeliveryLetterOuts_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconDeliveryLetterOutItems_SubconDeliveryLetterOutId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarmentSubconDeliveryLetterOuts",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.DropColumn(
                name: "UENItemId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.RenameTable(
                name: "GarmentSubconDeliveryLetterOuts",
                newName: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentSubconDeliveryLetterOuts_DLNo",
                table: "GarmentSubconDeliveryLetterOuts",
                newName: "IX_GarmentSubconDeliveryLetterOuts_DLNo");

            migrationBuilder.AddColumn<Guid>(
                name: "GarmentSubconDeliveryLetterOutIdentity",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarmentSubconDeliveryLetterOuts",
                table: "GarmentSubconDeliveryLetterOuts",
                column: "Identity");

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
    }
}
