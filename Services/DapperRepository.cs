using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;

namespace SoftWizard.Services
{
    public class DapperRepository<TEntity>(string connectionString) : IRepository<TEntity>
    {
        private readonly IDbConnection _dbConnection = new SqlConnection(connectionString);

        public IEnumerable<TEntity> GetAll()
        {
            return _dbConnection.Query<TEntity>("SELECT * FROM " + typeof(TEntity).Name);
        }

        public TEntity GetById(int id)
        {
            var result = _dbConnection.Query<TEntity>("SELECT * FROM " + typeof(TEntity).Name + " WHERE Id = @Id", new { Id = id }).FirstOrDefault();
            return result != null ? result : default;
        }

        public void Insert(TEntity entity)
        {
            _dbConnection.Execute($"INSERT INTO {typeof(TEntity).Name} VALUES (@Property1, @Property2, ...)", entity);
        }

        public void Update(TEntity entity)
        {
            _dbConnection.Execute($"UPDATE {typeof(TEntity).Name} SET Property1 = @Property1, Property2 = @Property2, ... WHERE Id = @Id", entity);
        }

        public void Delete(int id)
        {
            _dbConnection.Execute($"DELETE FROM {typeof(TEntity).Name} WHERE Id = @Id", new { Id = id });
        }

       
    }
}
