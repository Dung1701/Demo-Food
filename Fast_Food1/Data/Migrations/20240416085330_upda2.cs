using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class upda2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RatingAverage",
                table: "Food",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingAverage",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");
        }
    }
}
