using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleClassroom.API.Data;
using GoogleClassroom.API.DTOs.Material;
using GoogleClassroom.API.Models;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.API.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly ApplicationDbContext _context;

        public MaterialService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MaterialResponseDto> CreateMaterialAsync(int classroomId, int userId, CreateMaterialDto dto)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null || !classroom.Members.Any(m => m.UserId == userId && m.MembershipType == "Teacher"))
                throw new Exception("Classroom not found or you are not a teacher");

            var material = new Material
            {
                ClassroomId = classroomId,
                AuthorId = userId,
                Title = dto.Title,
                Description = dto.Description,
                AttachmentUrl = dto.AttachmentUrl
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return MapToResponse(material);
        }

        public async Task<IEnumerable<MaterialResponseDto>> GetClassroomMaterialsAsync(int classroomId, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You do not have access to this classroom");

            var materials = await _context.Materials
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                .Where(m => m.ClassroomId == classroomId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return materials.Select(MapToResponse);
        }

        public async Task<MaterialResponseDto> GetMaterialByIdAsync(int classroomId, int materialId, int userId)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == classroomId);

            if (classroom == null) throw new Exception("Classroom not found");

            if (!classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You do not have access to this classroom");

            var material = await _context.Materials
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == materialId && m.ClassroomId == classroomId);

            if (material == null) throw new Exception("Material not found");

            return MapToResponse(material);
        }

        private MaterialResponseDto MapToResponse(Material material)
        {
            return new MaterialResponseDto
            {
                Id = material.Id,
                ClassroomId = material.ClassroomId,
                TeacherId = material.AuthorId, // DTO alias
                Title = material.Title,
                Description = material.Description,
                AttachmentUrl = material.AttachmentUrl,
                CreatedAt = DateTime.SpecifyKind(material.CreatedAt, DateTimeKind.Utc),
                Comments = material.Comments?.Select(c => new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User?.FirstName + " " + c.User?.LastName,
                    Comment = c.Content,
                    CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)
                }).OrderBy(c => c.CreatedAt).ToList() ?? new List<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto>()
            };
        }

        public async Task<GoogleClassroom.API.DTOs.Announcement.CommentResponseDto> AddMaterialCommentAsync(int materialId, int userId, GoogleClassroom.API.DTOs.Announcement.CreateCommentDto dto)
        {
            var material = await _context.Materials
                .Include(m => m.Classroom)
                    .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(m => m.Id == materialId);

            if (material == null) throw new Exception("Material not found");

            if (!material.Classroom.Members.Any(m => m.UserId == userId))
                throw new Exception("You are not a member of this classroom");

            var comment = new MaterialComment
            {
                MaterialId = materialId,
                UserId = userId,
                Content = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.MaterialComments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Entry(comment).Reference(c => c.User).LoadAsync();

            return new GoogleClassroom.API.DTOs.Announcement.CommentResponseDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User.FirstName + " " + comment.User.LastName,
                Comment = comment.Content,
                CreatedAt = DateTime.SpecifyKind(comment.CreatedAt, DateTimeKind.Utc)
            };
        }
    }
}
