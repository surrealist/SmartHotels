using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreatFriends.SmartHotel.Migrations.Migrations
{
  public partial class update01 : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Users", x => x.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Users");
    }
  }
}
