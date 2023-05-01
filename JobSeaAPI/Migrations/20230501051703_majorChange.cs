using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class majorChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Status_Applications_ApplicationId",
                table: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Status_ApplicationId",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "CurrStatus",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "created",
                table: "Status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Status",
                newName: "StatusName");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Updates",
                columns: table => new
                {
                    UpdateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Updates", x => x.UpdateId);
                    table.ForeignKey(
                        name: "FK_Updates_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Updates_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Hired" },
                    { 2, "Rejected" },
                    { 3, "Interview Scheduled" },
                    { 4, "Applied" },
                    { 5, "Waiting" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Updates_ApplicationId",
                table: "Updates",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Updates_StatusId",
                table: "Updates",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Updates");

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "Status",
                newName: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CurrStatus",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "Status",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Status_ApplicationId",
                table: "Status",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Status_Applications_ApplicationId",
                table: "Status",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
