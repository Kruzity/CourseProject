using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CourseProjectUserApplication.Model;

namespace CourseProjectUserApplication.Repositories
{
    public class UserRepository : RepositoryBase, IUserCredentialRepository
    {
        public int AuthenticateUser(NetworkCredential userCredential)
        {
            int userid;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.CommandText =$"select [dbo].IsUserExist(@login, @password, 'user')";
                command.Parameters.AddWithValue("@login", userCredential.UserName);
                command.Parameters.AddWithValue("@password", userCredential.Password);
                command.Connection = connection;

                userid = (int)command.ExecuteScalar();

                connection.Close();

                return userid;
            }
        }
    }
}
