using EmpolyeeInfo.EmpolyeeInfo.Application.ServiceInterface;
using EmpolyeeInfo.EmpolyeeInfo.Domain.Models;

namespace EmpolyeeInfo.EmpolyeeInfo.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return _employeeRepository.GetAllEmployeesAsync();
        }

        public Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            return _employeeRepository.GetEmployeeByIdAsync(employeeId);
        }

        public Task UpdateEmployeeAsync(Employee employee)
        {
            return _employeeRepository.UpdateEmployeeAsync(employee);
        }

        public Task DeleteEmployeeAsync(int employeeId)
        {
            return _employeeRepository.DeleteEmployeeAsync(employeeId);
        }

        public Task ImportEmployeesFromXmlAsync(string xmlData)
        {
            return _employeeRepository.ImportEmployeesFromXmlAsync(xmlData);
        }
    }
}
