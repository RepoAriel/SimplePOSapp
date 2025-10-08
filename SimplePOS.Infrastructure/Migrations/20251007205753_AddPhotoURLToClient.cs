using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplePOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoURLToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoURL",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoURL",
                table: "Client");
        }
    }
}
