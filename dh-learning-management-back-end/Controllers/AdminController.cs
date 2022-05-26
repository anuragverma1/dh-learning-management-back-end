using dh_learning_management_back_end.Models;
using dh_learning_management_back_end.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dh_learning_management_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminRepository _adminRepository;

        public AdminController() => _adminRepository = new AdminRepository();

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public ActionResult<string> AddCourse(CourseDto request)
        {
            return Ok(_adminRepository.CreateCourse(request)
                ? "Created Successfully"
                : "Error while creating.Plz try again.");
        }

        [HttpPost("edit")]
        [Authorize(Roles = "admin")]
        public ActionResult<string> EditCourse(Course request)
        {
            return Ok(_adminRepository.EditCourse(request)
                ? "Edited Successfully"
                : "Error while Editing.Plz try again.");
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public ActionResult<string> DeleteCourse(Guid request)
        {
            return Ok(_adminRepository.DeleteCourse(request)
                ? "Deleted Successfully"
                : "Error while Deleting.Plz try again.");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetAllCourses()
        {
            return Ok(_adminRepository.GetCourses());
        }

        [HttpPost("assign")]
        [Authorize(Roles = "admin")]
        public ActionResult<AssignCourseResponseDto> AssignCourse(AssignCourseDto request)
        {
            var response = _adminRepository.Assign(request);
            if (response.Issuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet("report/{username}")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<AdminReportDto>> GetReport(string username)
        {
            return Ok(_adminRepository.Report(username));
        }
    }
}