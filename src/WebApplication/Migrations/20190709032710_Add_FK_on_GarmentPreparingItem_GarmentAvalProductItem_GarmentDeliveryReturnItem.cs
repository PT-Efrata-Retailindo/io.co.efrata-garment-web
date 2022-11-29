using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_FK_on_GarmentPreparingItem_GarmentAvalProductItem_GarmentDeliveryReturnItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "GarmentPreparings",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "GarmentPreparings",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UENNo",
                table: "GarmentPreparings",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentPreparings",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentPreparings",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentPreparingItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentPreparingItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentPreparingItems",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FabricType",
                table: "GarmentPreparingItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentPreparingItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "GarmentDeliveryReturns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitDONo",
                table: "GarmentDeliveryReturns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "GarmentDeliveryReturns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageName",
                table: "GarmentDeliveryReturns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageCode",
                table: "GarmentDeliveryReturns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReturnType",
                table: "GarmentDeliveryReturns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentDeliveryReturns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DRNo",
                table: "GarmentDeliveryReturns",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentDeliveryReturns",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentDeliveryReturnItems",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentAvalProducts",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentAvalProducts",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentAvalProductItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentAvalProductItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentAvalProductItems",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentAvalProductItems",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GarmentPreparingItems_GarmentPreparingId",
                table: "GarmentPreparingItems",
                column: "GarmentPreparingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentDeliveryReturnItems_DRId",
                table: "GarmentDeliveryReturnItems",
                column: "DRId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentAvalProductItems_APId",
                table: "GarmentAvalProductItems",
                column: "APId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentAvalProductItems_GarmentAvalProducts_APId",
                table: "GarmentAvalProductItems",
                column: "APId",
                principalTable: "GarmentAvalProducts",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentDeliveryReturnItems_GarmentDeliveryReturns_DRId",
                table: "GarmentDeliveryReturnItems",
                column: "DRId",
                principalTable: "GarmentDeliveryReturns",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentPreparingItems_GarmentPreparings_GarmentPreparingId",
                table: "GarmentPreparingItems",
                column: "GarmentPreparingId",
                principalTable: "GarmentPreparings",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentAvalProductItems_GarmentAvalProducts_APId",
                table: "GarmentAvalProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentDeliveryReturnItems_GarmentDeliveryReturns_DRId",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentPreparingItems_GarmentPreparings_GarmentPreparingId",
                table: "GarmentPreparingItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentPreparingItems_GarmentPreparingId",
                table: "GarmentPreparingItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentDeliveryReturnItems_DRId",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentAvalProductItems_APId",
                table: "GarmentAvalProductItems");

            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "GarmentPreparings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "GarmentPreparings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UENNo",
                table: "GarmentPreparings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentPreparings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentPreparings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentPreparingItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentPreparingItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentPreparingItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FabricType",
                table: "GarmentPreparingItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentPreparingItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitDONo",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageName",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageCode",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReturnType",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DRNo",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentDeliveryReturns",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentDeliveryReturnItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentDeliveryReturnItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentDeliveryReturnItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentDeliveryReturnItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentDeliveryReturnItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RONo",
                table: "GarmentAvalProducts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                table: "GarmentAvalProducts",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UomUnit",
                table: "GarmentAvalProductItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "GarmentAvalProductItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "GarmentAvalProductItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignColor",
                table: "GarmentAvalProductItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
