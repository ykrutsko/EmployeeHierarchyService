using System.Collections.Generic;

namespace EmployeeService
{
    public class Employee
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enable { get; set; }

        public int? ManagerID { get; set; }

        public List<Employee> Subordinates { get; set; }
    }
}
