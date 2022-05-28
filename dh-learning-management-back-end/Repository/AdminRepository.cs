using System.Data;
using Dapper;
using dh_learning_management_back_end.Models;
using Npgsql;

namespace dh_learning_management_back_end.Repository;

public class AdminRepository
{
    private readonly string _connectionString;

    public AdminRepository() =>
        _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=L&D";

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public bool CreateCourse(CourseDto request)
    {
        using var dbConnection = Connection;
        const string sQuery =
            @"INSERT INTO courses(coursename, courseduration, seatsavailable, coursestartdate, courseenddate, courseimgurl) VALUES (@coursename, @courseduration, @seatsavailable, @coursestartdate, @courseenddate, @courseimgurl);";
        dbConnection.Open();
        try
        {
            dbConnection.Execute(sQuery, request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool EditCourse(Course request)
    {
        using var dbConnection = Connection;
        const string sQuery =
            @"UPDATE courses SET coursename=@coursename, courseduration=@courseduration, seatsavailable=@seatsavailable, coursestartdate=@coursestartdate, courseenddate=@courseenddate, courseimgurl=@courseimgurl WHERE courseid=@courseid;";
        dbConnection.Open();
        try
        {
            dbConnection.Execute(sQuery, request);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteCourse(Guid courseid)
    {
        using var dbConnection = Connection;
        const string sQuery = @"DELETE FROM courses WHERE courseid=@courseid;";
        dbConnection.Open();
        try
        {
            dbConnection.Execute(sQuery, new { Courseid = courseid });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<Course> GetCourses()
    {
        using var dbConnection = Connection;
        const string sQuery = @"Select * from Courses";
        dbConnection.Open();
        return dbConnection.Query<Course>(sQuery);
    }

    public AssignCourseResponseDto Assign(AssignCourseDto request)
    {
        if (SeatsAvailable(request.Courseassigned))
        {
            using var dbConnection = Connection;
            const string sQuery =
                @"INSERT INTO assigned( username, courseassigned) VALUES (@username, @courseassigned);";
            dbConnection.Open();
            try
            {
                dbConnection.Execute(sQuery, request);
                return new AssignCourseResponseDto
                {
                    Message = "Assigned Successfully",
                    Issuccess = true
                };
            }
            catch
            {
                return new AssignCourseResponseDto
                {
                    Message = "Error while Assigning,try again later",
                    Issuccess = false
                };
            }
        }
        else
        {
            return new AssignCourseResponseDto
            {
                Message = "Seats Not Available",
                Issuccess = false
            };
        }
    }

    private bool SeatsAvailable(Guid courseid)
    {
        using var dbConnection = Connection;
        const string sQuery = @"Select seatsavailable from Courses where courseid=@courseid";
        dbConnection.Open();
        return dbConnection.QuerySingle<int>(sQuery, new { Courseid = courseid }) > 0;
    }

    public IEnumerable<AdminReportDto> Report(string username)
    {
        using var dbConnection = Connection;
        const string sQuery =
            @"select courses.coursename , courses.courseduration, courses.seatsavailable, courses.courseimgurl, assigned.status from courses inner join assigned on courses.courseid = assigned.courseassigned where assigned.username = @username;";
        dbConnection.Open();
        return dbConnection.Query<AdminReportDto>(sQuery, new { Username = username });
    }
}