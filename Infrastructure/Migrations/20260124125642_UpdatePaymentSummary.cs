using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpMonth",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ExpYear",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "PaymentSummary_ExpMonth",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentSummary_ExpYear",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentSummary_ExpMonth",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentSummary_ExpYear",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ExpMonth",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpYear",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
