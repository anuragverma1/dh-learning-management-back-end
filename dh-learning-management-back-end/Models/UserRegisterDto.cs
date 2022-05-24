namespace dh_learning_management_back_end.Models;

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Mobileno { get; set; }
    public string Manager { get; set; } = string.Empty;
    public string Profileimg { get; set; } = string.Empty;
}