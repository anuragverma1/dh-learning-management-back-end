using dh_learning_management_back_end.Models;
using dh_learning_management_back_end.Repository;
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
        public ActionResult<string> AddCourse(Course request)
        {
            return Ok(_adminRepository.CreateCourse(request)
                ? "Created Successfully"
                : "Error while creating.Plz try again.");
        }

        [HttpPost("edit")]
        public ActionResult<string> EditCourse(Course request)
        {
            return Ok(_adminRepository.EditCourse(request)
                ? "Edited Successfully"
                : "Error while Editing.Plz try again.");
        }

        [HttpDelete("delete")]
        public ActionResult<string> DeleteCourse(Guid request)
        {
            return Ok(_adminRepository.DeleteCourse(request)
                ? "Deleted Successfully"
                : "Error while Deleting.Plz try again.");
        }
    }
}