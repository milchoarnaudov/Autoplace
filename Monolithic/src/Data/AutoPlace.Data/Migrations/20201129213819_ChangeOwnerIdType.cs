using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoPlace.Data.Migrations
{
    public partial class ChangeOwnerIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autoparts_AspNetUsers_OwnerId1",
                table: "Autoparts");

            migrationBuilder.DropIndex(
                name: "IX_Autoparts_OwnerId1",
                table: "Autoparts");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Autoparts");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Autoparts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Autoparts_OwnerId",
                table: "Autoparts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Autoparts_AspNetUsers_OwnerId",
                table: "Autoparts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autoparts_AspNetUsers_OwnerId",
                table: "Autoparts");

            migrationBuilder.DropIndex(
                name: "IX_Autoparts_OwnerId",
                table: "Autoparts");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Autoparts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                table: "Autoparts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Autoparts_OwnerId1",
                table: "Autoparts",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Autoparts_AspNetUsers_OwnerId1",
                table: "Autoparts",
                column: "OwnerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
