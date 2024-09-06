using EmpolyeeInfo.EmpolyeeInfo.Application.Services;
using EmpolyeeInfo.EmpolyeeInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmpolyeeInfo.Pages
{
    public class SearchEmployeeModel : PageModel
    {
        private readonly EmployeeService _employeeService;
        public Employee Employee { get; private set; }

        public SearchEmployeeModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task OnPostAsync(int employeeId)
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
        }
    }

}
