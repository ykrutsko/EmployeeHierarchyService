using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace EmployeeService
{
    public class Service1 : IEmployeeService
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDb"].ConnectionString;

        public Employee GetEmployeeById(int id)
        {
            var employees = new List<Employee>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // NOTE:
                    // For simplicity in this test task, we load all employees into memory.
                    // In a real environment, this could be optimized with a recursive CTE query
                    // to fetch only the requested employee and their subordinates.
                    cmd.CommandText = "SELECT ID, Name, ManagerID, Enable FROM Employee";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ManagerID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                Enable = reader.GetBoolean(3),
                                Subordinates = new List<Employee>()
                            });
                        }
                    }
                }
            }

            var dict = new Dictionary<int, Employee>();
            foreach (var emp in employees)
            {
                dict[emp.ID] = emp;
            }

            foreach (var emp in employees)
            {
                if (emp.ManagerID.HasValue && dict.ContainsKey(emp.ManagerID.Value))
                {
                    dict[emp.ManagerID.Value].Subordinates.Add(emp);
                }
            }

            return dict.ContainsKey(id) ? dict[id] : null;
        }

        public void EnableEmployee(int id, bool enable)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Employee SET Enable=@Enable WHERE ID=@ID";
                    cmd.Parameters.AddWithValue("@Enable", enable ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}