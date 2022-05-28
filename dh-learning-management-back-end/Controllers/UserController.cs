using dh_learning_management_back_end.Models;
using dh_learning_management_back_end.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dh_learning_management_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AdminRepository _adminRepository;

        public UserController()
        {
            _userRepository = new UserRepository();
            _adminRepository = new AdminRepository();
        }

        [HttpPost("{username}")]
        public ActionResult<UserCoursesStatusDto> GetCoursesStatus(string username)
        {
            return Ok(_userRepository.GetStatus(username));
        }

        [HttpPost("courses/{coursename}")]
        public ActionResult<Course> GetCourses(string coursename)
        {
            return Ok(_userRepository.GetCourseInfo(coursename));
        }

        [HttpPost("courses/{username}")]
        public ActionResult<IEnumerable<AdminReportDto>> GetUserCourses(string username)
        {
            return Ok(_adminRepository.Report(username));
        }
    }
}
