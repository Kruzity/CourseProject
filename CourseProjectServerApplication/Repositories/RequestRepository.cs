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
    internal class RequestRepository : RepositoryBase, IRequestRepository
    {
        public void AddRequest(RequestModel request)
        {
            throw new NotImplementedException();
        }

        public void EditRequest(RequestModel request)
        {
            if (request == null) return;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();

                if(request.DoneDate!="")
                    command.CommandText = $"update requests set State='{request.State.ToString()}', DoneDate='{request.DoneDate}' where Id={request.RequestId}";
                else
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

        public List<RequestModel> GetByAll()
        {
            List<RequestModel> requestsList = new List<RequestModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = @"select * from requests";
                command.Connection = connection;

                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader != null)
                    {
                        while(dataReader.Read())
                        {
                            var a = (int)dataReader[0];
                            var b = (int)dataReader[1];
                            var c = (string)dataReader[2];
                            var d =(string)dataReader[3];
                            var e =(RequestState)Enum.Parse(typeof(RequestState), (string)dataReader[4]);
                            var j = ((DateTime)dataReader[5]).ToString("yyyy-MM-dd HH:mm:ss");
                            var g = (dataReader[6] == DBNull.Value) ? "" : ((DateTime)dataReader[6]).ToString("yyyy-MM-dd HH:mm:ss");

                            requestsList.Add(new RequestModel(a, b, c, d, e, j, g));
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(nameof(GetByAll) + " - " + ex.Message);
                }
                connection.Close();
            }
            return requestsList;
        }
        public void RemoveRequest(int id)
        {
            throw new NotImplementedException();
        }
    }
}
