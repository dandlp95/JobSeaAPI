using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class jobModalities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModalityId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Modalities",
                columns: table => new
                {
                    ModalityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalities", x => x.ModalityId);
                });

            migrationBuilder.InsertData(
                table: "Modalities",
                columns: new[] { "ModalityId", "Name" },
                values: new object[,]
                {
                    { 1, "On Site" },
                    { 2, "Hybrid" },
                    { 3, "Remote" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ModalityId",
                table: "Applications",
                column: "ModalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Modalities_ModalityId",
                table: "Applications",
                column: "ModalityId",
                principalTable: "Modalities",
                principalColumn: "ModalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Modalities_ModalityId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "Modalities");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ModalityId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ModalityId",
                table: "Applications");

            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Applications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
