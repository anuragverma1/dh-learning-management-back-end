namespace dh_learning_management_back_end.Models;

public class AdminReportDto
{
    public Guid Courseid { get; set; }
    public string Coursename { get; set; } = string.Empty;
    public int Courseduration { get; set; }
    public int Seatsavailable { get; set; }
    public string Courseimgurl { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
}