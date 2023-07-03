using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBangReact2.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocActive",
                table: "Doctors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocActive",
                table: "Doctors",
                type: "bit",
                nullable: true);
        }
    }
}
