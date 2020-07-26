using Abb.EmployeeDetails.Api.DataAccess;
using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDataAccess _employeeDataAccess;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeDataAccess employeeDataAccess, IMapper mapper)
        {
            _employeeDataAccess = employeeDataAccess;
            _mapper = mapper;
        }
        public async Task<int> CreateEmployee(EmployeeCreateViewModel employeeCreate)
        {
            Employees employee = _mapper.Map<Employees>(employeeCreate);

            int employeeId = await _employeeDataAccess.CreateEmployee(employee);

            return employeeId;
        }

        public async Task<bool?> DeleteEmployee(int employeeId)
        {
            bool? result = null;
            if (await GetEmployeeById(employeeId) != null)
            {
                result = await _employeeDataAccess.DeleteEmployee(employeeId);
            }
            return result;
        }

        public async Task<Employees> GetEmployeeById(int employeeId)
        {
            var employees = await _employeeDataAccess.GetEmployees();
            Employees employee = employees.Where(x => x.Id == employeeId).FirstOrDefault();
            return employee;
        }

        public async Task<IQueryable<Employees>> GetEmployees()
        {
            return await _employeeDataAccess.GetEmployees();
        }
    }
}
