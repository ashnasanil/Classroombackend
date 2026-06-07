using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoogleClassroom.API.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApplyDraftGradeToMissing",
                table: "Classrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClassworkOnStream",
                table: "Classrooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "InviteCodesEnabled",
                table: "Classrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MissingAssignmentDefaultGrade",
                table: "Classrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OverallGradeCalculation",
                table: "Classrooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "ShowDeletedItems",
                table: "Classrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOverallGradeToStudents",
                table: "Classrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StreamPostPermission",
                table: "Classrooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyDraftGradeToMissing",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "ClassworkOnStream",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "InviteCodesEnabled",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "MissingAssignmentDefaultGrade",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "OverallGradeCalculation",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "ShowDeletedItems",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "ShowOverallGradeToStudents",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "StreamPostPermission",
                table: "Classrooms");
        }
    }
}
