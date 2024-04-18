using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class up7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OrderId",
                table: "Comments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Order_OrderId",
                table: "Comments",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Order_OrderId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OrderId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Comments");
        }
    }
}
