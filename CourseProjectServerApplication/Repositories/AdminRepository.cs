using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CourseProjectServerApplication.Model;

namespace CourseProjectServerApplication.Repositories
{
    public class AdminRepository : RepositoryBase, IAdminCredentialRepository
    {
        public int AuthenticateAdmin(NetworkCredential adminCredential)
        {
            int userid;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.CommandText =$"select [dbo].IsUserExist(@login, @password, 'admin')";
                command.Parameters.AddWithValue("@login", adminCredential.UserName);
                command.Parameters.AddWithValue("@password", adminCredential.Password);
                command.Connection = connection;

                userid = (int)command.ExecuteScalar();

                connection.Close();

                return userid;
            }
        }
    }
}
