using Dapper;
using SoftWizard.Models;
using System.Data.SqlClient;

namespace SoftWizard.Services
{
    public class OkpdCategoryService(IConfiguration configuration, ILogger<OkpdCategoryService> logger) : IOkpdCategoryRepository
    {
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<OkpdCategoryService> _logger = logger;

        public async Task<int> AddAsync(OkpdCategory entity)
        {
            try
            {
                string sql = "Insert into OkpdCategories (Name,Razdel,Level,ParentId) VALUES (@Name,@Razdel,@Level,@ParentId)";
                using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
                connection.Open();
                int result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return 500;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                string sql = "DELETE FROM OkpdCategories WHERE Id = @Id";
                using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
                connection.Open();
                int result = await connection.ExecuteAsync(sql, new { Id = id });
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return 500;
            }
        }

        public async Task<IReadOnlyList<OkpdCategory>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM OkpdCategories";
                using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
                connection.Open();
                IEnumerable<OkpdCategory> result = await connection.QueryAsync<OkpdCategory>(sql);
                connection.Close();
                return result.ToList();
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return null;
            }
        }

        public async Task<OkpdCategory> GetByIdAsync(int id)
        {
            try
            {
                string sql = "SELECT * FROM OkpdCategories WHERE Id = @Id";
                using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
                connection.Open();
                OkpdCategory? result = await connection.QuerySingleOrDefaultAsync<OkpdCategory>(sql, new { Id = id });
                connection.Close();
                return result;

            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return null;
            }
        }

        public async Task<int> UpdateAsync(OkpdCategory entity)
        {
            try
            {
                string sql = "UPDATE OkpdCategories SET Id = @Id, Name = @Name, Razdel = @Razdel, Level = @Level, ParentId = @ParentId WHERE Id = @Id";
                using var connection = new SqlConnection(configuration.GetConnectionString("connStr"));
                connection.Open();
                int result = await connection.ExecuteAsync(sql, entity);
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return 500;
            }
        }
    }
}
