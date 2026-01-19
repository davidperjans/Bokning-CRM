using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingType_SoftDelete_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BookingTypes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTypes_BusinessId",
                table: "BookingTypes",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTypes_BusinessId_IsActive",
                table: "BookingTypes",
                columns: new[] { "BusinessId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookingTypes_BusinessId",
                table: "BookingTypes");

            migrationBuilder.DropIndex(
                name: "IX_BookingTypes_BusinessId_IsActive",
                table: "BookingTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BookingTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}
