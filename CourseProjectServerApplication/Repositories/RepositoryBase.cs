using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectServerApplication.Repositories
{
    public abstract class RepositoryBase
    {
        string _connectionString;

        public RepositoryBase()
        {
            _connectionString = @"Server=localhost;Initial Catalog=RequestProcessingApplicationDB;Integrated Security=true";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
