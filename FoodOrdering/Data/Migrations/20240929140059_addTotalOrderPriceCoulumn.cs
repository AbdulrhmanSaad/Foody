using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrdering.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTotalOrderPriceCoulumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalOrderPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalOrderPrice",
                table: "Orders");
        }
    }
}
