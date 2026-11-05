using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class categoryConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_NameAr",
                table: "Categories",
                column: "NameAr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_NameAr",
                table: "Categories");
        }
    }
}
