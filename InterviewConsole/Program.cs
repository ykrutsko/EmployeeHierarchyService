using System;
using System.Data;
using System.Data.SqlClient;

namespace InterviewConsole
{
    class Program
    {
        private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=EmployeeServiceDBTest;Integrated Security=True;";

        static void Main(string[] args)
        {
            DataTable dt = GetQueryResult("SELECT * FROM Employee");

            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"ID: {row["ID"]}, Name: {row["Name"]}, ManagerID: {row["ManagerID"]}, Enable: {row["Enable"]}");
            }

            Console.WriteLine("\nDone");
            Console.ReadLine();
        }

        private static DataTable GetQueryResult(string query)
        {
            var dt = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
