using System.Data;
using Dapper;
using dh_learning_management_back_end.Models;
using Npgsql;

namespace dh_learning_management_back_end.Repository;

public class AdminRepository
{
    private readonly string _connectionString;

    public AdminRepository() =>
        _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=L&D";

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

    public bool DeleteCourse(Guid request)
    {
        using var dbConnection = Connection;
        const string sQuery = @"DELETE FROM courses WHERE courseid=@courseid;";
        dbConnection.Open();
        try
        {
            dbConnection.Execute(sQuery, new { Courseid = request });
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
        return dbConnection.Query<Course>(sQuery);
    }
}