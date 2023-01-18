using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSeaAPI.Migrations
{
    /// <inheritdoc />
    public partial class uniqueconstraintsemailcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_UserId",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_status_applications_ApplicationId",
                table: "status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_status",
                table: "status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_applications",
                table: "applications");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "status",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "applications",
                newName: "Applications");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Status",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "currStatus",
                table: "Status",
                newName: "CurrStatus");

            migrationBuilder.RenameIndex(
                name: "IX_status_ApplicationId",
                table: "Status",
                newName: "IX_Status_ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_applications_UserId",
                table: "Applications",
                newName: "IX_Applications_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "StatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Status_Applications_ApplicationId",
                table: "Status",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Status_Applications_ApplicationId",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_email",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "status");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "applications");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "status",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "CurrStatus",
                table: "status",
                newName: "currStatus");

            migrationBuilder.RenameIndex(
                name: "IX_Status_ApplicationId",
                table: "status",
                newName: "IX_status_ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_UserId",
                table: "applications",
                newName: "IX_applications_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_status",
                table: "status",
                column: "StatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_applications",
                table: "applications",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_UserId",
                table: "applications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_status_applications_ApplicationId",
                table: "status",
                column: "ApplicationId",
                principalTable: "applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
