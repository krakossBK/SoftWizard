using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SoftWizard.Models
{
    public interface IUserRepository
    {
        void Create(User user);
        void Delete(int id);
        User Get(int id);
        List<User> GetUsers();
        void Update(User user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = "Data Source=ms-sql-10.in-solve.ru;Initial Catalog=1gb_vladimirpiter;Integrated Security=False;User ID=1gb_olga-arsi;Password=4uC8s47Ke6i5;TrustServerCertificate=True";
        public UserRepository(string conn)
        {
            connectionString = conn;
        }
        public List<User> GetUsers()
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                var result = db.Query<User>("SELECT * FROM Users").ToList();
                var countUser = result.Count;
                return result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message;

                return null;
            }
        }

        public User Get(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                var result = db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
                int countUser = result != null ? result.Id : 100;

                return result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message;

                return null;
            }
        }

        public void Create(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Users (NameUser, Email) VALUES(@NameUser, @Email)";
                db.Execute(sqlQuery, user);

                // если мы хотим получить id добавленного пользователя
                //var sqlQuery = "INSERT INTO Users (Name,Email) VALUES(@Name, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                //user.Id = userId.Value;
            }
        }

        public void Update(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Users SET NameUser = @NameUser, Email = @Email WHERE Id = @Id";
                db.Execute(sqlQuery, user);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                string sqlQuery = "DELETE FROM Users WHERE Id = @id";
                int result = db.Execute(sqlQuery, new { id });
                int countUser = result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message; 
            }
        }
    }
}
