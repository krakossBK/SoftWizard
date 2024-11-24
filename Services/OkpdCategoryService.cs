using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using SoftWizard.AppCode;
using SoftWizard.Models;
using System.Data;

namespace SoftWizard.Services
{
    public class OkpdCategoryService(ILogger<OkpdCategoryService> logger,
                                     IMemoryCache memoryCache,
                                     IDbConnection dbConnection) : IOkpdCategoryRepository
    {
        public ILogger<OkpdCategoryService> _logger = logger;
        private readonly IMemoryCache cache = memoryCache;
        private readonly IDbConnection _dbConnection = dbConnection;


        public async Task<int> AddAsync(OkpdCategory entity)
        {
            try
            {
                string sql = "Insert into OkpdCategories (Name,Razdel,Level,ParentId) VALUES (@Name,@Razdel,@Level,@ParentId)";
                return await _dbConnection.ExecuteAsync(sql, entity);
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
                return await _dbConnection.ExecuteAsync(sql, new { Id = id });
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
                IEnumerable<OkpdCategory>? result;

                bool resultCache = cache.TryGetValue(CacheKeys.OkpdCategory, value: out List<OkpdCategory> okpdCategory);

                if (!resultCache)
                {
                    string sql = "SELECT * FROM OkpdCategories";
                    result = await _dbConnection.QueryAsync<OkpdCategory>(sql);

                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                        SlidingExpiration = TimeSpan.FromMinutes(1),
                        Size = 1024,
                    };
                    cache.Set(CacheKeys.OkpdCategory, result, cacheEntryOptions);
                    _logger.LogInformation($"{result.Count()} штук записей извлечены из базы данных");
                }
                else
                    result = cache.Get(CacheKeys.OkpdCategory) as IEnumerable<OkpdCategory>;

                return result.ToList(); // Get the data from database
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
                    result = await _dbConnection.QuerySingleOrDefaultAsync<OkpdCategory>(sql, new { Id = id });
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
                    string resultName = result != null ? result.Name : "";
                    _logger.LogInformation($"{resultName} извлечен из базы данных");
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
                return await _dbConnection.ExecuteAsync(sql, entity);
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
