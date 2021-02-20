using Microsoft.EntityFrameworkCore.Migrations;

namespace GreatFriends.SmartHoltel.Services.Data.Migrations
{
    public partial class update02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_TypeCode",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "TypeCode",
                table: "Rooms",
                newName: "RoomTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_TypeCode",
                table: "Rooms",
                newName: "IX_Rooms_RoomTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeCode",
                table: "Rooms",
                column: "RoomTypeCode",
                principalTable: "RoomTypes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeCode",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomTypeCode",
                table: "Rooms",
                newName: "TypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomTypeCode",
                table: "Rooms",
                newName: "IX_Rooms_TypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_TypeCode",
                table: "Rooms",
                column: "TypeCode",
                principalTable: "RoomTypes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
