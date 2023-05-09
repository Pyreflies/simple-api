using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Transactions;

namespace Cafe.Controllers.DBHandlers
{
    public class SqlDBContext
    {
        private readonly IConfiguration _configuration;

/*        public void DoSomething()
        {
            // Retrieve the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Create a new SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Do something with the connection
            }
        }*/

        public static string getConnection()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["ConnectionStrings:DefaultConnection"];
        }
        public static List<Dictionary<string, object>> SelectQuery(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(getConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        if (dt.Rows.Count <= 0) throw new ArgumentException("ไม่พบข้อมูล");
                        foreach (DataRow dr in dt.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }
                        con.Close();
                        return rows;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new ArgumentException(ex.Message);
                return null;
            }

        }
        public static DataTable Getdatatable(string sql)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(getConnection()))
                {
                    String cmd = sql;
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        conn.Open();
                        adapter.SelectCommand = new SqlCommand(cmd, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        conn.Close();
                        return dt;
                    };
                };
            }
            catch (Exception ex)
            {
                throw ex;
                //DataTable dt = new DataTable();
                //return dt;
            }
        }

        public static List<string> InsertUpdateDelete(string sql)
        {
            try
            {
                List<string> ListID = new List<string>();
                using (var connection = new SqlConnection(getConnection()))
                {
                    string[] listSql = sql.Split(';');
                    foreach (var Sql in listSql)
                    {
                        if (Sql != "")
                        {
                            using (SqlCommand cmd = new SqlCommand(Sql, connection))
                            {
                                connection.Open();
                                using (TransactionScope scope = new TransactionScope())
                                {
                                    var modified = cmd.ExecuteScalar();
                                    if (connection.State == System.Data.ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                    scope.Complete();
                                    ListID.Add(modified.ToString());
                                }
                            }
                        }
                    }
                }
                return ListID;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static int ExecuteNonQuery(string sql)
        {
            try
            {
                int re = 0;
                using (SqlConnection connection = new SqlConnection(getConnection()))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        re = command.ExecuteNonQuery();
                    };
                };
                return re;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
