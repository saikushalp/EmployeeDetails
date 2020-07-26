using Abb.EmployeeDetails.Api.Controllers;
using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using Abb.EmployeeDetails.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.MSTest.Controllers
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

        [TestClass]
        public class GetEmployees : EmployeesControllerTest
        {
            [TestMethod]
            public async Task Should_Get_All_Employees()
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployees()).ReturnsAsync(EmployeesDataMock.AsQueryable());

                //Act
                var result = await _controllerUnderTest.GetEmployees();

                //Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                Assert.IsNotNull(((OkObjectResult)result).Value);
                Assert.IsTrue(((IEnumerable<Employees>)((OkObjectResult)result).Value).Any());
            }
        }

        [TestClass]
        public class GetEmployeeById : EmployeesControllerTest
        {
            [DataTestMethod]
            [DataRow(1)]
            [DataRow(2)]
            [DataRow(3)]
            public async Task Should_Get_Employee_By_Id(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
                Assert.AreEqual(employeeId, ((Employees)((OkObjectResult)actionResult).Value).Id);
            }

            [DataTestMethod]
            [DataRow(-1)]
            [DataRow(0)]
            public async Task Should_Return_BadRequest_Result(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
                Assert.IsNotNull(((BadRequestObjectResult)actionResult).Value);
            }

            [TestMethod]
            public async Task Should_Return_NotFound_Result()
            {
                //Arrange
                int employeeId = 6;
                _employeeServiceMock.Setup(x => x.GetEmployeeById(employeeId))
                    .ReturnsAsync(EmployeesDataMock.FirstOrDefault(x => x.Id == employeeId));

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult,typeof(NotFoundObjectResult));
                Assert.IsNotNull(((NotFoundObjectResult)actionResult).Value);
            }
        }

        [TestClass]
        public class DeleteEmployee : EmployeesControllerTest
        {
            [DataTestMethod]
            [DataRow(1)]
            [DataRow(2)]
            [DataRow(3)]
            public async Task Should_Delete_Employee_By_Id(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync(true);

                //Act
                var actionResult = await _controllerUnderTest.Delete(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult,typeof(OkObjectResult));
                Assert.IsNotNull(((OkObjectResult)actionResult).Value);
            }

            [DataTestMethod]
            [DataRow(-1)]
            [DataRow(0)]
            public async Task Should_Return_BadRequest_Result(int employeeId)
            {
                //Arrange
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync((bool?)null);

                //Act
                var actionResult = await _controllerUnderTest.Delete(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));
                Assert.IsNotNull(((BadRequestObjectResult)actionResult).Value);
            }

            [TestMethod]
            public async Task Should_Return_NotFound_Result()
            {
                //Arrange
                int employeeId = 6;
                _employeeServiceMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync((bool?)null);

                //Act
                var actionResult = await _controllerUnderTest.GetEmployeeById(employeeId);

                //Assert
                Assert.IsInstanceOfType(actionResult,typeof(NotFoundObjectResult));
                Assert.IsNotNull(((NotFoundObjectResult)actionResult).Value);
            }
        }

        [TestClass]
        public class CreateEmployee : EmployeesControllerTest
        {
            [TestMethod]
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
                Assert.IsInstanceOfType(actionResult, typeof(CreatedResult));
                Assert.IsNotNull(((CreatedResult)actionResult).Value);
            }

            [TestMethod]
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
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
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
