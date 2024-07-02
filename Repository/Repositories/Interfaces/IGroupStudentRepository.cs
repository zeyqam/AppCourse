using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupStudentRepository : IBaseRepository<GroupStudents>
    {
        Task<int> CountStudentsInGroup(int groupId);
        Task CreateAsync(GroupStudents groupStudents);
        Task<GroupStudents> FindByStudentAndGroupAsync(int studentId, int groupId);
        Task UpdateAsync(GroupStudents groupStudents);
    }
}
