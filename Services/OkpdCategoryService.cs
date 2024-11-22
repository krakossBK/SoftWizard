using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using SoftWizard.Models;

namespace SoftWizard.Services
{
    public class OkpdCategoryService(IConfiguration configuration, ILogger<OkpdCategoryService> logger, IMemoryCache memoryCache) : IOkpdCategoryRepository
    {
        private readonly IConfiguration configuration = configuration;
        public ILogger<OkpdCategoryService> _logger = logger;
        private readonly IMemoryCache cache = memoryCache;

        public async Task<int> AddAsync(OkpdCategory entity)
        {

            int result;
            try
            {
                string sql = "Insert into OkpdCategories (Name,Razdel,Level,ParentId) VALUES (@Name,@Razdel,@Level,@ParentId)";
                var connString = configuration.GetConnectionString("connStr");
                SqlConnection sqlConn = new(connString);
                using SqlConnection connection = sqlConn;
                connection.Open();
                result = await connection.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                result = 500;
            }
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int result;
            try
            {
                string sql = "DELETE FROM OkpdCategories WHERE Id = @Id";
                var connString = configuration.GetConnectionString("connStr");
                SqlConnection sqlConn = new(connString);
                using SqlConnection connection = sqlConn;
                connection.Open();
                result = await connection.ExecuteAsync(sql, new { Id = id });
                connection.Close();
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                result = 500;
            }
            return result;
        }

        public async Task<IReadOnlyList<OkpdCategory>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM OkpdCategories";
                var connString = configuration.GetConnectionString("connStr");
                SqlConnection sqlConn = new(connString);
                using SqlConnection connection = sqlConn;
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
                OkpdCategory? result;

                // пытаемся получить данные из кэша
                cache.TryGetValue(id, out OkpdCategory? okpdCategory);
                // если данные не найдены в кэше
                if (okpdCategory == null)
                {
                    // обращаемся к базе данных
                    string sql = "SELECT * FROM OkpdCategories WHERE Id = @Id";
                    var connString = configuration.GetConnectionString("connStr");
                    SqlConnection sqlConn = new(connString);
                    using SqlConnection connection = sqlConn;
                    connection.Open();
                    result = await connection.QuerySingleOrDefaultAsync<OkpdCategory>(sql, new { Id = id });
                    connection.Close();
                    // если okpdCategory найден, то добавляем в кэш - время кэширования 1 минутa
                    if (result != null)
                    {
                        // определяем параметры кэширования
                        var cacheOptions = new MemoryCacheEntryOptions()
                        {
                            // кэширование в течение 1 минуты
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                            // низкий приоритет
                            Priority = 0,
                        };
                        // определяем коллбек при удалении записи из кэша
                        var callbackRegistration = new PostEvictionCallbackRegistration();

                        callbackRegistration.EvictionCallback =
                            (object key, object? value, EvictionReason reason, object? state) => _logger.LogInformation($"запись {id} устарела");
                        cacheOptions.PostEvictionCallbacks.Add(callbackRegistration);

                        cache.Set(result.Id, result, cacheOptions);
                    }
                       

                    _logger.LogInformation($"{result.Name} извлечен из базы данных");
                }
                else
                    return okpdCategory;

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
                var connString = configuration.GetConnectionString("connStr");
                SqlConnection sqlConn = new(connString);
                using SqlConnection connection = sqlConn;
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
