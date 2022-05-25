using System.ComponentModel.DataAnnotations;

namespace dh_learning_management_back_end.Models;

public class Course
{
    public Guid Courseid { get; set; }
    public string Coursename { get; set; } = string.Empty;
    public int Courseduration { get; set; }
    public int Seatsavailable { get; set; }
    [DataType(DataType.Date)] public DateTime Coursestartdate { get; set; }
    [DataType(DataType.Date)] public DateTime Courseenddate { get; set; }
}