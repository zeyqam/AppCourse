using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Admin.Groups;
using Service.DTOs.Admin.Students;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto request)
        {
            await _studentService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { Response = "Succesfull" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mappedStudents = await _studentService.GetAllWithInclude();
            return Ok(mappedStudents);
        }
        [HttpPost("{studentId}/add-group")]
        public async Task<IActionResult> AddGroup([FromRoute]int studentId, [FromBody] AddGroupDto request)
        {
            try
            {
                await _studentService.AddGroupStudentAsync(studentId, request.GroupId);
                return Ok(new { Response = "Group added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Response = ex.Message });
            }
        }
        [HttpPut("{studentId}/change-group")]
        public async Task<IActionResult> ChangeGroup([FromRoute]int studentId, [FromBody] ChangeGroupDto request)
        {
            try
            {
                await _studentService.ChangeStudentGroupAsync(studentId, request.OldGroupId, request.NewGroupId);
                return Ok(new { Response = "Group changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Response = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] StudentEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _studentService.EditAsync(id, request);
                return Ok(new { Response = "Successful" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _studentService.DeleteAsync(id);
                return Ok(new { Response = "Successful" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
