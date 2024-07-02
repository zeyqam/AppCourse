using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupStudentRepository : IBaseRepository<GroupStudents>
    {
        Task<int> CountStudentsInGroup(int groupId);
        
        Task<GroupStudents> FindByStudentAndGroupAsync(int studentId, int groupId);
        
    }
}
