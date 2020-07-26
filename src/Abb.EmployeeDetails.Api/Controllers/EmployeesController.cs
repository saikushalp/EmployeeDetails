using Abb.EmployeeDetails.Api.Models.ViewModels;
using Abb.EmployeeDetails.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get All Employees Details
        /// </summary>
        /// <returns>List of Employees</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();

            return Ok(employees);
        }


        /// <summary>
        /// Get Employee Details by EmployeeId
        /// </summary>
        /// <param name="id">EmployeeId</param>
        /// <returns>Employee</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            if (id > 0)
            {
                var employee = await _employeeService.GetEmployeeById(id);

                if (employee != null)
                {
                    return Ok(employee);
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = $"Employee_With_Id_{id}_NotFound"
                    });
                }
            }

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = $"Invalid_Employee_Id_{id}"
            });
        }

        /// <summary>
        /// Create new Employee
        /// </summary>
        /// <param name="employeeCreate">Employee Create request object</param>
        /// <returns>Employee Id</returns>
        [HttpPost]
        [Authorize]        
        [ProducesResponseType((int)HttpStatusCode.Created)]        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody]EmployeeCreateViewModel employeeCreate)
        {
            int employeeId = await _employeeService.CreateEmployee(employeeCreate);

            if (employeeId > 0)
            {
                return Created("", new { employeeId = employeeId });
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete Employee by Employee Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Employee Deletion Status</returns>
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")]int employeeId)
        {
            if (employeeId > 0)
            {
                bool? result = await _employeeService.DeleteEmployee(employeeId);

                if (result != null)
                {
                    return Ok(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = $"Employee_With_Id_{employeeId}_Deleted_Successfully"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = $"Employee_With_Id_{employeeId}_NotFound"
                    });
                }
            }

            return BadRequest(new
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = $"Invalid_Employee_Id_{employeeId}"
            });
        }
    }
}