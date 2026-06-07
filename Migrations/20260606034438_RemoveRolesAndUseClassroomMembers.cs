using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoogleClassroom.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRolesAndUseClassroomMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Users_TeacherId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmissions_Users_StudentId",
                table: "AssignmentSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassroomMembers_Users_StudentId",
                table: "ClassroomMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Users_TeacherId",
                table: "Classrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_TeacherId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Users_TeacherId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Users",
                newName: "RecoveryEmail");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Materials",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_TeacherId",
                table: "Materials",
                newName: "IX_Materials_AuthorId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Grades",
                newName: "GraderId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                newName: "IX_Grades_GraderId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Classrooms",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Classrooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BannerImage",
                table: "Classrooms",
                newName: "ThemeColor");

            migrationBuilder.RenameIndex(
                name: "IX_Classrooms_TeacherId",
                table: "Classrooms",
                newName: "IX_Classrooms_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "ClassroomMembers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassroomMembers_StudentId",
                table: "ClassroomMembers",
                newName: "IX_ClassroomMembers_UserId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "AssignmentSubmissions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentSubmissions_StudentId",
                table: "AssignmentSubmissions",
                newName: "IX_AssignmentSubmissions_UserId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Assignments",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                newName: "IX_Assignments_AuthorId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Announcements",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements",
                newName: "IX_Announcements_AuthorId");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MembershipType",
                table: "ClassroomMembers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClassroomInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    InvitedEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvitedById = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomInvitations_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomInvitations_Users_InvitedById",
                        column: x => x.InvitedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomInvitations_ClassroomId",
                table: "ClassroomInvitations",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomInvitations_InvitedById",
                table: "ClassroomInvitations",
                column: "InvitedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Users_AuthorId",
                table: "Announcements",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_AuthorId",
                table: "Assignments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmissions_Users_UserId",
                table: "AssignmentSubmissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassroomMembers_Users_UserId",
                table: "ClassroomMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Users_CreatedBy",
                table: "Classrooms",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_GraderId",
                table: "Grades",
                column: "GraderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Users_AuthorId",
                table: "Materials",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Users_AuthorId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_AuthorId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmissions_Users_UserId",
                table: "AssignmentSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassroomMembers_Users_UserId",
                table: "ClassroomMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Users_CreatedBy",
                table: "Classrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_GraderId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Users_AuthorId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "ClassroomInvitations");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "ClassroomMembers");

            migrationBuilder.RenameColumn(
                name: "RecoveryEmail",
                table: "Users",
                newName: "ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Materials",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_AuthorId",
                table: "Materials",
                newName: "IX_Materials_TeacherId");

            migrationBuilder.RenameColumn(
                name: "GraderId",
                table: "Grades",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_GraderId",
                table: "Grades",
                newName: "IX_Grades_TeacherId");

            migrationBuilder.RenameColumn(
                name: "ThemeColor",
                table: "Classrooms",
                newName: "BannerImage");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Classrooms",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Classrooms",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Classrooms_CreatedBy",
                table: "Classrooms",
                newName: "IX_Classrooms_TeacherId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClassroomMembers",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassroomMembers_UserId",
                table: "ClassroomMembers",
                newName: "IX_ClassroomMembers_StudentId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AssignmentSubmissions",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentSubmissions_UserId",
                table: "AssignmentSubmissions",
                newName: "IX_AssignmentSubmissions_StudentId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Assignments",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_AuthorId",
                table: "Assignments",
                newName: "IX_Assignments_TeacherId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Announcements",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_AuthorId",
                table: "Announcements",
                newName: "IX_Announcements_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Teacher" },
                    { 3, "Student" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Users_TeacherId",
                table: "Announcements",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmissions_Users_StudentId",
                table: "AssignmentSubmissions",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassroomMembers_Users_StudentId",
                table: "ClassroomMembers",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Users_TeacherId",
                table: "Classrooms",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_TeacherId",
                table: "Grades",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Users_TeacherId",
                table: "Materials",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
