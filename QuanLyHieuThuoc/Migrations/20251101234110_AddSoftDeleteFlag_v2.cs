using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyHieuThuoc.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFlag_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PhieuNhaps");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoaDons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PhieuNhaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoaDons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
