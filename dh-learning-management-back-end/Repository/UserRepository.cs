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


        internal UserCoursesStatusDto GetStatus(string username)
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

        internal object? GetCourseInfo(string coursename)
        {
            using var dbConnection = Connection;
            const string sQuery = @"Select * from Courses where coursename=@coursename";
            dbConnection.Open();
            return dbConnection.Query<Course>(sQuery, new { Coursename = coursename });
        }
    }
}
