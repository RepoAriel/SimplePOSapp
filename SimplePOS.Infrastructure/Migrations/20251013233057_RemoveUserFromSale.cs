using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplePOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFromSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sale");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sale",
                type: "varchar(450)",
                maxLength: 450,
                nullable: true);
        }
    }
}
