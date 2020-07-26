using Abb.EmployeeDetails.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.DataAccess
{
    public interface IEmployeeDataAccess
    {
        /// <summary>
        /// Get All Employee Details
        /// </summary>
        /// <returns>List of Employees</returns>
        Task<IQueryable<Employees>> GetEmployees();

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employee">Employee Details</param>
        /// <returns>Employee Id</returns>
        Task<int> CreateEmployee(Employees employee);

        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Employee Delete Status</returns>
        Task<bool> DeleteEmployee(int employeeId);
    }
}
