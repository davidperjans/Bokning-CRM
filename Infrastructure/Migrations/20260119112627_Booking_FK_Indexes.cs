using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Booking_FK_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingTypeId",
                table: "Bookings",
                column: "BookingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BusinessId_ResourceId_StartTimeUtc_EndTimeUtc",
                table: "Bookings",
                columns: new[] { "BusinessId", "ResourceId", "StartTimeUtc", "EndTimeUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BusinessId_UserId_Status",
                table: "Bookings",
                columns: new[] { "BusinessId", "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings",
                column: "BookingTypeId",
                principalTable: "BookingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Resources_ResourceId",
                table: "Bookings",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Resources_ResourceId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingTypeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BusinessId_ResourceId_StartTimeUtc_EndTimeUtc",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BusinessId_UserId_Status",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings");
        }
    }
}
