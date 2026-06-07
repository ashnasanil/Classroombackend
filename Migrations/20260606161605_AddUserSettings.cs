using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoogleClassroom.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AllowEmailNotifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CommentsOnYourPosts = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CommentsThatMentionYou = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PrivateCommentsOnWork = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WorkAndPostsFromTeachers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReturnedWorkAndGrades = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InvitationsToJoinClasses = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DueDateReminders = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LateSubmissions = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Resubmissions = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InvitationsToCoteach = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScheduledPostPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShowDisplayNameOnHomepage = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
