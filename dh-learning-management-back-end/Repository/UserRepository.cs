using Dapper;
using dh_learning_management_back_end.Models;
using Npgsql;
using System.Data;

namespace dh_learning_management_back_end.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository() =>
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=L&D";

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);


        public UserCoursesStatusDto GetStatus(string username)
        {
            using var dbConnection = Connection;
            const string sQuery1 = @"SELECT COUNT(STATUS) FROM ASSIGNED WHERE USERNAME=@username";
            const string sQuery2 = @"SELECT COUNT(STATUS) FROM ASSIGNED WHERE USERNAME=@username AND STATUS = 'completed';";
            dbConnection.Open();
            var all = dbConnection.QuerySingle<int>(sQuery1, new { Username = username });
            var comp = dbConnection.QuerySingle<int>(sQuery2, new { Username = username });
            return new UserCoursesStatusDto
            {
                AllCourses = all,
                Completedcourses = comp,
                Pendingcourses = all - comp
            };
        }

        public Course GetCourseInfo(string coursename)
        {
            using var dbConnection = Connection;
            const string sQuery = @"Select * from Courses where coursename=@coursename";
            dbConnection.Open();
            return dbConnection.QuerySingle<Course>(sQuery, new { Coursename = coursename });
        }

        public IEnumerable<Course> GetCourses(string username)
        {
            using var dbConnection = Connection;
            const string sQuery = @"Select courses.courseid, courses.coursename, courses.courseduration, courses.courseimgurl, assigned.status from Courses inner join assigned on courses.courseid = assigned.courseassigned where assigned.username = @username;";
            dbConnection.Open();
            return dbConnection.Query<Course>(sQuery, new { Username = username });
        }

        public void ChangeStatus(UserMarkDto request)
        {
            using var dbConnection = Connection;
            const string sQuery = @"UPDATE assigned set status='completed' WHERE username=@username and courseassigned=@courseid;";
            dbConnection.Open();
            dbConnection.Execute(sQuery, request);
        }

        public IEnumerable<AdminReportDto> Report(string username, string status)
        {
            using var dbConnection = Connection;
            string sQuery;
            if (status == "all")
            {
                sQuery =
                @"select courses.courseid, courses.coursename , courses.courseduration, courses.seatsavailable, courses.courseimgurl, assigned.status from courses inner join assigned on courses.courseid = assigned.courseassigned where assigned.username = @username";
            }
            else
            {
                sQuery =
                    @"select courses.courseid, courses.coursename , courses.courseduration, courses.seatsavailable, courses.courseimgurl, assigned.status from courses inner join assigned on courses.courseid = assigned.courseassigned where assigned.username = @username and assigned.status=@status;";
            }
            dbConnection.Open();
            return dbConnection.Query<AdminReportDto>(sQuery, new { Username = username, Status = status });
        }
    }
}
