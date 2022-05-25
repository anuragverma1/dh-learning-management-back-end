namespace dh_learning_management_back_end.Models;

public class AssignCourseDto
{
    public string Username { get; set; } = string.Empty;
    public Guid Courseassigned { get; set; }
}