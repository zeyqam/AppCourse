using Service.DTOs.Admin.Students;

namespace Service.Services.Interfaces
{
    public interface IStudentService
    {
        Task CreateAsync(StudentCreateDto model);
        Task<IEnumerable<StudentDto>> GetAllWithInclude();
        Task AddGroupStudentAsync(int studentId, int groupId);
        Task ChangeStudentGroupAsync(int studentId, int oldGroupId, int newGroupId);
    }
}
