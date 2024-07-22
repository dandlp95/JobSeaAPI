using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class separateTimeAndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Updates",
                newName: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Updates",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Updates",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Updates");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Updates",
                newName: "notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Updates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
