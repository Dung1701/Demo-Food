using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class update05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Food",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountToday",
                table: "Food",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "Food",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Food",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "DiscountToday",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Food");
        }
    }
}
