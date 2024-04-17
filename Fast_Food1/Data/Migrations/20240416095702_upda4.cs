using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class upda4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasRated",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRated",
                table: "Order");
        }
    }
}
