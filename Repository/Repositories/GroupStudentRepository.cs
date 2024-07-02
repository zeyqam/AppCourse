using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class GroupStudentRepository : BaseRepository<GroupStudents>, IGroupStudentRepository
    {
        public GroupStudentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountStudentsInGroup(int groupId)
        {
            return await _context.Set<GroupStudents>().CountAsync(gs => gs.GroupId == groupId);
        }

        public async Task CreateAsync(GroupStudents groupStudents)
        {
            await _context.Set<GroupStudents>().AddAsync(groupStudents);
            await _context.SaveChangesAsync();
        }

        public async Task<GroupStudents> FindByStudentAndGroupAsync(int studentId, int groupId)
        {
            return await _context.Set<GroupStudents>()
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.GroupId == groupId);
        }

        public async Task UpdateAsync(GroupStudents groupStudents)
        {
            _context.Set<GroupStudents>().Update(groupStudents);
            await _context.SaveChangesAsync();
        }
    }
}
