using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoPlace.Data.Migrations
{
    public partial class AddContactForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    FullName = table.Column<string>(maxLength: 255, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Topic = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactForms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactForms_IsDeleted",
                table: "ContactForms",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactForms");
        }
    }
}
