using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Day21_Inventory_System.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Purchases",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Purchases");
        }
    }
}
