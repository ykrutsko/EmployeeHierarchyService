# EmployeeHierarchyService

A simple C# WCF service that manages employees and their hierarchical relationships.

This project demonstrates:
- Building and returning hierarchical employee structures (manager → subordinates)
- Enabling/disabling employees via database updates
- Working with ADO.NET and SQL Server
- Clean and readable code with clear data mapping

## Features

- **GetEmployeeById** — returns an employee and all of their subordinates as a hierarchical tree  
- **EnableEmployee** — enables or disables an employee record  
- Uses in-memory hierarchy reconstruction after loading data from the database  

## Project Structure

```
EmployeeService/
├── Employee.cs             # Employee model with recursive Subordinates list
├── IEmployeeService.cs     # WCF service contract
├── Service1.cs             # Service implementation with SQL logic
├── App.config              # Connection string configuration
```

## Database Schema

Run the following SQL script to create and populate the Employee table:

```sql
CREATE TABLE Employee (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ManagerID INT NULL,
    Enable BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ManagerID) REFERENCES Employee(ID)
);

-- Sample data
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Alice', NULL, 1);
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Bob', 1, 1);
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Charlie', 1, 1);
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Diana', 2, 1);
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Eve', 2, 0);
INSERT INTO Employee (Name, ManagerID, Enable) VALUES ('Frank', 3, 1);
```

## Configuration

Set the connection string in `App.config`:

```xml
<connectionStrings>
  <add name="EmployeeDb" 
       connectionString="Data Source=YOUR_SERVER;Initial Catalog=YOUR_DATABASE;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Example Request

```
GET yourdomain/EmployeeService.svc/GetEmployeeById?id=1
```

## Example Response

```json
{
  "ID": 1,
  "Name": "Alice",
  "Enable": true,
  "Subordinates": [
    {
      "ID": 2,
      "Name": "Bob",
      "Enable": true,
      "Subordinates": [
        {
          "ID": 4,
          "Name": "Diana",
          "Enable": true,
          "Subordinates": []
        },
        {
          "ID": 5,
          "Name": "Eve",
          "Enable": false,
          "Subordinates": []
        }
      ]
    },
    {
      "ID": 3,
      "Name": "Charlie",
      "Enable": true,
      "Subordinates": [
        {
          "ID": 6,
          "Name": "Frank",
          "Enable": true,
          "Subordinates": []
        }
      ]
    }
  ]
}
```

This structure clearly shows the hierarchical tree of employees, with nested `Subordinates` arrays.

## Notes

For simplicity in this test task, all employees are loaded into memory and the hierarchy is built locally.  
In a real environment, this could be optimized using a recursive CTE query to fetch only the requested employee and their subordinates.

## Author

**Yurii Krutsko**  
.NET Developer  
yuri.krutsko@gmail.com 
https://www.linkedin.com/in/yurii-krutsko/
