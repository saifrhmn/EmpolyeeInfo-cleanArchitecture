using EmpolyeeInfo.EmpolyeeInfo.Domain.Models;

namespace EmpolyeeInfo.EmpolyeeInfo.Application.ServiceInterface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int employeeId);
        Task ImportEmployeesFromXmlAsync(string xmlData);
    }
}
