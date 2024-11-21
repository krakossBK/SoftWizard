using Dapper;
using SoftWizard.Models;
using System.Data.SqlClient;

namespace SoftWizard.Services
{
    public class OkpdCategoryService(IConfiguration configuration) : IOkpdCategoryRepository
    {
        private readonly IConfiguration configuration = configuration;

        public async Task<int> AddAsync(OkpdCategory entity)
        {
            string sql = "Insert into OkpdCategories (Code,Description) VALUES (@Code,@Description)";
            using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
            connection.Open();
            int result = await connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            string sql = "DELETE FROM OkpdCategories WHERE Id = @Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
            connection.Open();
            int result = await connection.ExecuteAsync(sql, new { Id = id });
            connection.Close();
            return result;
        }

        public async Task<IReadOnlyList<OkpdCategory>> GetAllAsync()
        {
            string sql = "SELECT * FROM OkpdCategories";
            using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
            connection.Open();
            IEnumerable<OkpdCategory> result = await connection.QueryAsync<OkpdCategory>(sql);
            connection.Close();
            return result.ToList();
        }

        public async Task<OkpdCategory> GetByIdAsync(int id)
        {
            string sql = "SELECT * FROM OkpdCategories WHERE Id = @Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
            connection.Open();
            OkpdCategory? result = await connection.QuerySingleOrDefaultAsync<OkpdCategory>(sql, new { Id = id });
            connection.Close();
            return result;
        }

        public async Task<int> UpdateAsync(OkpdCategory entity)
        {
            string sql = "UPDATE OkpdCategories SET Code = @Code, Description = @Description WHERE Id = @Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
            connection.Open();
            int result = await connection.ExecuteAsync(sql, entity);
            connection.Close();
            return result;
        }
    }
}
