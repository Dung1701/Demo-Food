using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fast_Food1.Data.Migrations
{
    /// <inheritdoc />
    public partial class update04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PaymentMethods_PaymentMethodId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentMethodId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentMethodsId",
                table: "Order",
                column: "PaymentMethodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PaymentMethods_PaymentMethodsId",
                table: "Order",
                column: "PaymentMethodsId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PaymentMethods_PaymentMethodsId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentMethodsId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentMethodId",
                table: "Order",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PaymentMethods_PaymentMethodId",
                table: "Order",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id");
        }
    }
}
