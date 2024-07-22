using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class jobDetailsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobDetails",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobDetails",
                table: "Applications");
        }
    }
}
