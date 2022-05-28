namespace dh_learning_management_back_end.Models
{
    public class UserInfoDto
    {
        public string Username { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Mobileno { get; set; }
        public string Manager { get; set; } = string.Empty;
        public string Profileimg { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}

