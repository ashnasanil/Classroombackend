using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoogleClassroom.API.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Levels",
                table: "Classrooms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Levels",
                table: "Classrooms");
        }
    }
}
