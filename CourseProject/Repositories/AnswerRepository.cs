using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CourseProjectUserApplication.Model;

namespace CourseProjectUserApplication.Repositories
{
    internal class AnswerRepository : RepositoryBase, IAnswersRepository
    {
        public void EditAnswer(int? id)
        {
            if (id == null) return;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();

                command.CommandText = $"update Answers set IsReaded=1 where Id={id}";
                command.Connection = connection;

                try
                {
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(nameof(EditAnswer) + " - " + ex.Message);
                }
                connection.Close();
            }
        }

        public List<AnswerModel> GetAnswersByRequestId(int id)
        {
            List<AnswerModel> tmpList = new List<AnswerModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();

                command.CommandText = $"select * from [dbo].RequestAnswers({id.ToString()})";

                command.Connection = connection;

                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            var d = (int)dataReader[0];
                            var a = (string)dataReader[1];
                            var b = ((DateTime)dataReader[2]).ToString();
                            var c = (bool)dataReader[3];

                            tmpList.Add(new AnswerModel(d, a, b, c));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(nameof(GetAnswersByRequestId) + " - " + ex.Message);
                }
                connection.Close();
                return tmpList;
            }
        }
        
    }
}
