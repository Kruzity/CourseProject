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
    internal class RequestRepository : RepositoryBase, IRequestRepository
    {
        public void AddRequest(RequestModel request)
        {
            if (request == null) return;

            using(var connection = GetConnection())
            using(var command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = $"insert into requests " +
                    $"values ({request.UserId}, '{request.Title}', '{request.Message}', '{request.State.ToString()}', '{request.RequestedDate.ToString()}', NULL)";
                command.Connection = connection;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(nameof(AddRequest) + " - " + ex.Message);
                }
                connection.Close();
            }
        }

        public void EditRequest(RequestModel request)
        {
            if (request == null) return;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();

                command.CommandText = $"update requests set State='{request.State.ToString()}' where Id={request.RequestId}";
                command.Connection = connection;

                try
                {
                    command.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(nameof(EditRequest) + " - " + ex.Message);
                }
                connection.Close();
            }
        }
        public List<RequestModel> GetByUserId(int id)
        {
            List<RequestModel> tmpList = new List<RequestModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();

                command.CommandText = $"select * from [dbo].GetRequestsAndUnreadMessageCountByUser({id.ToString()})";

                command.Connection = connection;

                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            var a = (int)dataReader[0];
                            var b = (int)dataReader[1];
                            var c = (string)dataReader[2];
                            var d = (string)dataReader[3];
                            var e = (RequestState)Enum.Parse(typeof(RequestState), (string)dataReader[4]);
                            var j = ((DateTime)dataReader[5]).ToString();
                            var g = (dataReader[6] == DBNull.Value) ? "" : ((DateTime)dataReader[6]).ToString();
                            var u = (int)dataReader[7] == 0 ? " " : ((int)dataReader[7]).ToString();

                            tmpList.Add(new RequestModel(a, b, c, d, e, j, g, u));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(nameof(GetByUserId) + " - " + ex.Message);
                }
                connection.Close();
                return tmpList;
            }
        }
    }
}
