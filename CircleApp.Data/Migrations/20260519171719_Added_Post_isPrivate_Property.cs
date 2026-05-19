using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircleApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_Post_isPrivate_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "Posts");
        }
    }
}
