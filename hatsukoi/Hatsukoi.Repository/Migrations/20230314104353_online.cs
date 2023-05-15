using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatsukoi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class online : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImg",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "商品圖片");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImg",
                table: "OrderDetail");
        }
    }
}
