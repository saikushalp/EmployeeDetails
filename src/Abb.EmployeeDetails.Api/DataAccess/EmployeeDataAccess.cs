using Abb.EmployeeDetails.Api.DataAccess.Context;
using Abb.EmployeeDetails.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.DataAccess
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {
        private readonly ApplicationContext _dbContext;
        public EmployeeDataAccess(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateEmployee(Employees employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee.Id;
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            int result = 0;
            Employees employee =  _dbContext.Employees.Where(x => x.Id == employeeId).FirstOrDefault();

            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);

                result = await _dbContext.SaveChangesAsync();
            }

            return result > 0;
        }

        public async Task<IQueryable<Employees>> GetEmployees()
        {
            return await Task.FromResult(_dbContext.Employees.AsQueryable());
        }
    }
}
