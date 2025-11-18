using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusTypeAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "AvageStar",
                table: "Book",
                newName: "AvarageStar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvarageStar",
                table: "Book",
                newName: "AvageStar");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shop",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rating",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderDetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Category",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Book",
                type: "text",
                nullable: true);
        }
    }
}
