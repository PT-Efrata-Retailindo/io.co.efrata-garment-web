using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_SampleRequest_Config_SampleRequestNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentSampleRequests_SampleRequestNo",
                table: "GarmentSampleRequests");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleRequests_RONoSample",
                table: "GarmentSampleRequests",
                column: "RONoSample",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentSampleRequests_RONoSample",
                table: "GarmentSampleRequests");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleRequests_SampleRequestNo",
                table: "GarmentSampleRequests",
                column: "SampleRequestNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }
    }
}
