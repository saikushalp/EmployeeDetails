using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Get All Employee Details
        /// </summary>
        /// <returns>List of Employee Details</returns>
        Task<IQueryable<Employees>> GetEmployees();

        /// <summary>
        /// Get Employee Details by Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Employee Details</returns>
        Task<Employees> GetEmployeeById(int employeeId);

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employeeCreate">Employee Create</param>
        /// <returns>Employee Id</returns>
        Task<int> CreateEmployee(EmployeeCreateViewModel employeeCreate);

        /// <summary>
        /// Delete Employee by Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Returns null if Employee not found else returns bool</returns>
        Task<bool?> DeleteEmployee(int employeeId);
    }
}
