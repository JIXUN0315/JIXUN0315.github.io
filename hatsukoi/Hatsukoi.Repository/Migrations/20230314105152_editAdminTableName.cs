using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatsukoi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class editAdminTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewer_Administrator",
                table: "Reviewer");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "管理員Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "管理員的使用者ID"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "管理員名稱")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewer_Administrator",
                table: "Reviewer",
                column: "AdministratorId",
                principalTable: "Admin",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewer_Administrator",
                table: "Reviewer");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "管理員Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "管理員名稱"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "管理員的使用者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewer_Administrator",
                table: "Reviewer",
                column: "AdministratorId",
                principalTable: "Administrator",
                principalColumn: "Id");
        }
    }
}
