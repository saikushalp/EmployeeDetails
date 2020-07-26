using Abb.EmployeeDetails.Api.Controllers;
using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using Abb.EmployeeDetails.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Abb.EmployeeDetails.Tests.Controllers
{
    public class EmployeesControllerTest
    {
        private readonly EmployeesController _controllerUnderTest;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        public EmployeesControllerTest()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();

            _controllerUnderTest = new EmployeesController(_employeeServiceMock.Object);
        }

        public class GetEmployees : EmployeesControllerTest
        {
            [Fact]
            public async Task Should_Get_All_Employees()
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployees()).ReturnsAsync(EmployeesDataMock.AsQueryable());

                //Act
                var result = await _controllerUnderTest.GetEmployees();

                //Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okObjectResult.Value);
                Assert.True(((IEnumerable<Employees>)okObjectResult.Value).Any());
            }
        }

        public class GetEmployeeById : EmployeesControllerTest
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(3)]
            public async Task Should_Get_Employee_By_Id(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                var result = Assert.IsType<OkObjectResult>(actionResult);
                Assert.Equal(employeeId, ((Employees)result.Value).Id);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public async Task Should_Return_BadRequest_Result(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                var result = Assert.IsType<BadRequestObjectResult>(actionResult);
                Assert.NotNull(result.Value);
            }

            [Fact]            
            public async Task Should_Return_NotFound_Result()
            {
                //Arrange
                int employeeId = 6;
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                var result = Assert.IsType<NotFoundObjectResult>(actionResult);
                Assert.NotNull(result.Value);
            }
        }

        public class DeleteEmployee : EmployeesControllerTest
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(3)]
            public async Task Should_Delete_Employee_By_Id(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync(true);

                //Act
                var actionResult = await _controllerUnderTest.Delete(employeeId);

                //Assert
                var result = Assert.IsType<OkObjectResult>(actionResult);
                Assert.NotNull(result.Value);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public async Task Should_Return_BadRequest_Result(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync((bool?)null);

                //Act
                var actionResult = await _controllerUnderTest.Delete(employeeId);

                //Assert
                var result = Assert.IsType<BadRequestObjectResult>(actionResult);
                Assert.NotNull(result.Value);
            }

            [Fact]
            public async Task Should_Return_NotFound_Result()
            {
                //Arrange
                int employeeId = 6;
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync((bool?)null);

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                var result = Assert.IsType<NotFoundObjectResult>(actionResult);
                Assert.NotNull(result.Value);
            }
        }

        public class CreateEmployee : EmployeesControllerTest
        {
            [Fact]
            public async Task Should_Create_Employee()
            {
                //Arrange
                EmployeeCreateViewModel employeeCreate = new EmployeeCreateViewModel
                {
                    FirstName = "John",
                    LastName = "Smith",
                    City = "Newyork",
                    DateOfBirth = DateTime.Now.AddYears(-20)
                };
                _employeeServiceMock.Setup(x => x.CreateEmployee(employeeCreate)).ReturnsAsync(4);

                //Act
                var actionResult = await _controllerUnderTest.Create(employeeCreate);

                //Assert
                var result = Assert.IsType<CreatedResult>(actionResult);
                Assert.NotNull(result.Value);
            }

            [Fact]
            public async Task Should_Return_BadRequest()
            {
                //Arrange
                EmployeeCreateViewModel employeeCreate = new EmployeeCreateViewModel
                {
                    FirstName = "John",
                    LastName = "Smith",
                    City = "Newyork",
                    DateOfBirth = DateTime.Now.AddYears(-20)
                };
                _employeeServiceMock.Setup(x => x.CreateEmployee(employeeCreate)).ReturnsAsync(-1);

                //Act
                var actionResult = await _controllerUnderTest.Create(employeeCreate);

                //Assert
                Assert.IsType<BadRequestResult>(actionResult);                
            }
        }

        #region DataMock
        protected static List<Employees> EmployeesDataMock => new List<Employees>
        {
            new Employees
            {
                Id = 1, FirstName = "John Smith", LastName = "Paul", City = "Newyork", DateOfBirth = DateTime.Now.AddYears(-20)
            },

            new Employees
            {
                Id = 2, FirstName = "William Smith", LastName = "Daniel", City = "Delhi", DateOfBirth = DateTime.Now.AddYears(-25)
            },

            new Employees
            {
                Id = 3, FirstName = "William Smith", LastName = "Daniel", City = "Delhi", DateOfBirth = DateTime.Now.AddYears(-25)
            }
        };
        #endregion
    }
}
