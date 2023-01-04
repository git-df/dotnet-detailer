using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("CCConnectionString");
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_conString);
    }
}
