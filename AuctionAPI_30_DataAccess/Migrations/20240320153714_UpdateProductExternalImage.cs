using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionAPI_30_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductExternalImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ImageIsExternal",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageIsExternal",
                table: "Products");
        }
    }
}
