using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Students;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IGroupStudentRepository _groupStudentRepo;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepo,
                              IGroupStudentRepository groupStudentRepo,
                              IMapper mapper,
                              IGroupRepository groupRepo)
        {
            _studentRepo = studentRepo;
            _groupStudentRepo = groupStudentRepo;
            _mapper = mapper;
            _groupRepo = groupRepo;
        }

        public async Task CreateAsync(StudentCreateDto model)
        {
            var data = _mapper.Map<Student>(model);
            await _studentRepo.CreateAsync(data);

            foreach (var id in model.GroupIds)
            {
               await AddGroupStudentAsync(data.Id, id);
            }
        }

        public async Task<IEnumerable<StudentDto>> GetAllWithInclude()
        {
           var students =  await _studentRepo.FindAllWithIncludes()
                .Include(m => m.GroupStudents)
                .ThenInclude(m=>m.Group)
                .ToListAsync();
            var mappedStudents = _mapper.Map<List<StudentDto>>(students);
            return mappedStudents;
        }

        public async Task AddGroupStudentAsync(int studentId, int groupId)
        {
            var group = await _groupRepo.FindByIdAsync(groupId);
            if (group.Capacity > await _groupStudentRepo.CountStudentsInGroup(groupId))
            {
                await _groupStudentRepo.CreateAsync(new GroupStudents
                {
                    StudentId = studentId,
                    GroupId = groupId
                });
            }
            else
            {
                throw new Exception($"Group with id {groupId} is full.");
            }
        }
        public async Task ChangeStudentGroupAsync(int studentId, int oldGroupId, int newGroupId)
        {
            var newGroup = await _groupRepo.FindByIdAsync(newGroupId);
            if (newGroup.Capacity <= await _groupStudentRepo.CountStudentsInGroup(newGroupId))
            {
                throw new Exception($"New group with id {newGroupId} is full.");
            }

            var groupStudent = await _groupStudentRepo.FindByStudentAndGroupAsync(studentId, oldGroupId);
            if (groupStudent != null)
            {
                groupStudent.GroupId = newGroupId;
                await _groupStudentRepo.UpdateAsync(groupStudent);
            }
            else
            {
                throw new Exception($"Student with id {studentId} is not in group with id {oldGroupId}.");
            }
        }
    }
}
