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

        public UserController()
        {
            _userRepository = new UserRepository();
        }

        [HttpGet("{username}")]
        public ActionResult<UserCoursesStatusDto> GetCoursesStatus(string username)
        {
            return Ok(_userRepository.GetStatus(username));
        }

        [HttpGet("courses/{coursename}")]
        public ActionResult<Course> GetCourses(string coursename)
        {
            return Ok(_userRepository.GetCourseInfo(coursename));
        }

        [HttpGet("coursesinfo")]
        public ActionResult<IEnumerable<AdminReportDto>> GetUserCourses(string username, string status)
        {
            return Ok(_userRepository.Report(username, status));
        }

        [HttpPost("markcomplete")]
        public ActionResult MarkComplete(UserMarkDto request)
        {
            _userRepository.ChangeStatus(request);
            return Ok();
        }

        [HttpGet("mycourses")]
        public ActionResult<IEnumerable<Course>> GetMyCourses(string username)
        {
            return Ok(_userRepository.GetCourses(username));
        }
    }
}
