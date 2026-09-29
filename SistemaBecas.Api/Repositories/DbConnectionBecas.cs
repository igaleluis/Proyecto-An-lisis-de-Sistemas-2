using Npgsql;
namespace SistemaBecas.Api.Repositories
{
    public class DbConnectionBecas
    {
        private readonly IConfiguration _configuration;
        public DbConnectionBecas(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public NpgsqlConnection CreateConnection()
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            return new NpgsqlConnection(connectionString);
        }
    }
}
