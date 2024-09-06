using EmpolyeeInfo.EmpolyeeInfo.Application.Services;
using EmpolyeeInfo.EmpolyeeInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmpolyeeInfo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EmployeeService _employeeService;
        public IEnumerable<Employee> Employees { get; private set; }

        public IndexModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task OnGetAsync()
        {
            Employees = await _employeeService.GetAllEmployeesAsync();
        }
    }

}