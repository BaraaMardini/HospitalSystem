using Microsoft.Extensions.Configuration;

namespace ConnectionString
{
    public class connectionString
    {
        public static string _connectionString;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}