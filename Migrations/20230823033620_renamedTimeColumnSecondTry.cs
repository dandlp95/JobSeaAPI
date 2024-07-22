using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class renamedTimeColumnSecondTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Updates",
                newName: "EventTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventTime",
                table: "Updates",
                newName: "Time");
        }
    }
}
