using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Resource_SoftDelete_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Resources",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_BusinessId",
                table: "Resources",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_BusinessId_IsActive",
                table: "Resources",
                columns: new[] { "BusinessId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_BusinessId_Type",
                table: "Resources",
                columns: new[] { "BusinessId", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resources_BusinessId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_BusinessId_IsActive",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_BusinessId_Type",
                table: "Resources");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Resources",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}
