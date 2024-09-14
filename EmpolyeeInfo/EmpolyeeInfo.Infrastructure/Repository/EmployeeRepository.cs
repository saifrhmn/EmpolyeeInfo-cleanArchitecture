using EmpolyeeInfo.EmpolyeeInfo.Application.ServiceInterface;
using EmpolyeeInfo.EmpolyeeInfo.Domain.Models;
using Oracle.ManagedDataAccess.Client;
using System.Xml;

namespace EmpolyeeInfo.EmpolyeeInfo.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var command = new OracleCommand("SELECT * FROM Employees", connection);
            var reader = await command.ExecuteReaderAsync();

            var employees = new List<Employee>();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeID = reader.GetInt32(0),
                    FirstName = reader.GetString(1),    
                    LastName = reader.GetString(2),
                    Division = reader.GetString(3),
                    Building = reader.GetString(4),
                    Title = reader.GetString(5),
                    Room = reader.GetString(6)
                });
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT EmployeeID, FirstName, LastName, Division, Building, Title, Room FROM Employees WHERE EmployeeID = :EmployeeID";

                using var command = new OracleCommand(query, connection);
                command.Parameters.Add(new OracleParameter("EmployeeID", employeeId));

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Employee
                    {
                        EmployeeID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Division = reader.GetString(3),
                        Building = reader.GetString(4),
                        Title = reader.GetString(5),
                        Room = reader.GetString(6)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or return a specific error object
                throw new ApplicationException("Error fetching employee by ID", ex);
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using var connection = new OracleConnection(_connectionString);
            try
            {
                await connection.OpenAsync();

                string query = @"UPDATE Employees 
                         SET FirstName = :FirstName, 
                             LastName = :LastName, 
                             Division = :Division, 
                             Building = :Building, 
                             Title = :Title, 
                             Room = :Room
                         WHERE EmployeeID = :EmployeeID";

                using var command = new OracleCommand(query, connection);

                // Add parameters to the query
                command.Parameters.Add(new OracleParameter("FirstName", employee.FirstName));
                command.Parameters.Add(new OracleParameter("LastName", employee.LastName));
                command.Parameters.Add(new OracleParameter("Division", employee.Division));
                command.Parameters.Add(new OracleParameter("Building", employee.Building));
                command.Parameters.Add(new OracleParameter("Title", employee.Title));
                command.Parameters.Add(new OracleParameter("Room", employee.Room));
                command.Parameters.Add(new OracleParameter("EmployeeID", employee.EmployeeID));

                // Execute the update command
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new Exception("Update failed. No rows affected.");
                }
            }
            catch (OracleException ex)
            {
                // Handle Oracle-specific exceptions here
                Console.WriteLine($"Oracle error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            using var connection = new OracleConnection(_connectionString);
            try
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Employees WHERE EmployeeID = :EmployeeID";

                using var command = new OracleCommand(query, connection);

                // Add parameter to the query
                command.Parameters.Add(new OracleParameter("EmployeeID", employeeId));

                // Execute the delete command
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new Exception("Delete failed. No rows affected.");
                }
            }
            catch (OracleException ex)
            {
                // Handle Oracle-specific exceptions here
                Console.WriteLine($"Oracle error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task ImportEmployeesFromXmlAsync(string xmlData)
        {
            using var connection = new OracleConnection(_connectionString);
            try
            {
                await connection.OpenAsync();

                // Parse the XML data
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                var employees = doc.SelectNodes("/Employees/Employee");

                foreach (XmlNode employeeNode in employees)
                {
                    string? firstName = employeeNode["FirstName"]?.InnerText;
                    string? lastName = employeeNode["LastName"]?.InnerText;
                    string? division = employeeNode["Division"]?.InnerText;
                    string? building = employeeNode["Building"]?.InnerText;
                    string? title = employeeNode["Title"]?.InnerText;
                    string? room = employeeNode["Room"]?.InnerText;

                    string query = @"INSERT INTO Employees (FirstName, LastName, Division, Building, Title, Room) 
                             VALUES (:FirstName, :LastName, :Division, :Building, :Title, :Room)";

                    using var command = new OracleCommand(query, connection);
                    command.Parameters.Add(new OracleParameter("FirstName", firstName));
                    command.Parameters.Add(new OracleParameter("LastName", lastName));
                    command.Parameters.Add(new OracleParameter("Division", division));
                    command.Parameters.Add(new OracleParameter("Building", building));
                    command.Parameters.Add(new OracleParameter("Title", title));
                    command.Parameters.Add(new OracleParameter("Room", room));

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (OracleException ex)
            {
                // Handle Oracle-specific exceptions here
                Console.WriteLine($"Oracle error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
