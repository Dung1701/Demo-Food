using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_FoodId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_FoodId",
                table: "OrderDetail",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_FoodId",
                table: "OrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_FoodId",
                table: "OrderDetail",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");
        }
    }
}
