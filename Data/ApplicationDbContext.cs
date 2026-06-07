using Microsoft.EntityFrameworkCore;
using GoogleClassroom.API.Models;

namespace GoogleClassroom.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Classroom> Classrooms { get; set; } = null!;
        public DbSet<ClassroomMember> ClassroomMembers { get; set; } = null!;
        public DbSet<ClassroomInvitation> ClassroomInvitations { get; set; } = null!;
        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<AnnouncementComment> AnnouncementComments { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; } = null!;
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<UserSettings> UserSettings { get; set; } = null!;
        public DbSet<AssignmentComment> AssignmentComments { get; set; } = null!;
        public DbSet<SubmissionComment> SubmissionComments { get; set; } = null!;
        public DbSet<MaterialComment> MaterialComments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<UserSettings>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Classroom relationships
            modelBuilder.Entity<Classroom>()
                .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedClassrooms)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassroomMember relationships
            modelBuilder.Entity<ClassroomMember>()
                .HasOne(cm => cm.Classroom)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassroomMember>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.ClassroomMemberships)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassroomInvitation relationships
            modelBuilder.Entity<ClassroomInvitation>()
                .HasOne(ci => ci.Classroom)
                .WithMany()
                .HasForeignKey(ci => ci.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassroomInvitation>()
                .HasOne(ci => ci.InvitedBy)
                .WithMany()
                .HasForeignKey(ci => ci.InvitedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Announcement relationships
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Classroom)
                .WithMany(c => c.Announcements)
                .HasForeignKey(a => a.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Announcements)
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnnouncementComment relationships
            modelBuilder.Entity<AnnouncementComment>()
                .HasOne(ac => ac.Announcement)
                .WithMany(a => a.Comments)
                .HasForeignKey(ac => ac.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnnouncementComment>()
                .HasOne(ac => ac.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(ac => ac.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment relationships
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Classroom)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // AssignmentSubmission relationships
            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(s => s.Assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material relationships
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Classroom)
                .WithMany(c => c.Materials)
                .HasForeignKey(m => m.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade relationships
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.AssignmentSubmission)
                .WithOne(s => s.Grade)
                .HasForeignKey<Grade>(g => g.AssignmentSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification relationships
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // AssignmentComment relationships
            modelBuilder.Entity<AssignmentComment>()
                .HasOne(ac => ac.Assignment)
                .WithMany(a => a.Comments)
                .HasForeignKey(ac => ac.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssignmentComment>()
                .HasOne(ac => ac.User)
                .WithMany()
                .HasForeignKey(ac => ac.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // SubmissionComment relationships
            modelBuilder.Entity<SubmissionComment>()
                .HasOne(sc => sc.AssignmentSubmission)
                .WithMany(s => s.Comments)
                .HasForeignKey(sc => sc.AssignmentSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubmissionComment>()
                .HasOne(sc => sc.User)
                .WithMany()
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaterialComment relationships
            modelBuilder.Entity<MaterialComment>()
                .HasOne(mc => mc.Material)
                .WithMany(m => m.Comments)
                .HasForeignKey(mc => mc.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaterialComment>()
                .HasOne(mc => mc.User)
                .WithMany()
                .HasForeignKey(mc => mc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
