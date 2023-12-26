using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CourseProjectServerApplication.Model;

namespace CourseProjectServerApplication.Repositories
{
    internal class AnswerRepository : RepositoryBase, IAnswersRepository
    {
        public void AddAnswerr(AnswerModel answer, int requestId)
        {
            if (answer == null) return;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = $"insert into answers values ({requestId}, '{answer.Message}', '{answer.Date}', 0)";
                command.Connection = connection;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(nameof(AddAnswerr) + " - " + ex.Message);
                }
                connection.Close();
            }
        }
    }
}
