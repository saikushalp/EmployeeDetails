using Abb.EmployeeDetails.Api.DataAccess;
using Abb.EmployeeDetails.Api.Mappers;
using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using Abb.EmployeeDetails.Api.Services;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Abb.EmployeeDetails.Tests.Services
{
    public class EmployeeServiceTest
    {
        private readonly Mock<IEmployeeDataAccess> _employeeDataAccessMock;
        private readonly IMapper _mapperMock;
        private readonly EmployeeService _serviceUnderTest;

        public EmployeeServiceTest()
        {
            _employeeDataAccessMock = new Mock<IEmployeeDataAccess>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapperMock = config.CreateMapper();

            _serviceUnderTest = new EmployeeService(_employeeDataAccessMock.Object, _mapperMock);
        }

        public class CreateEmployee : EmployeeServiceTest
        {
            [Fact]
            public async Task Should_Return_EmployeeId()
            {
                //Arrange
                int expectedEmployeeId = 1;
                EmployeeCreateViewModel employeeCreate = new EmployeeCreateViewModel
                {
                    FirstName = "John",
                    LastName = "Smith",
                    City = "Newyork",
                    DateOfBirth = DateTime.Now.AddYears(-20)
                };
                _employeeDataAccessMock.Setup(x => x.CreateEmployee(It.IsAny<Employees>()))
                    .ReturnsAsync(expectedEmployeeId);

                //Act
                int actualResult = await _serviceUnderTest.CreateEmployee(employeeCreate);

                //Assert
                Assert.Equal(expectedEmployeeId, actualResult);
            }
        }

        public class DeleteEmployee : EmployeeServiceTest
        {
            [Theory]
            [InlineData(100, null)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            public async Task Should_Return_Delete_Status(int employeeId, bool? expectedResult)
            {
                //Arrange
                _employeeDataAccessMock.Setup(x => x.GetEmployees()).ReturnsAsync(EmployeesDataMock.AsQueryable());                
                _employeeDataAccessMock.Setup(x => x.DeleteEmployee(employeeId))
                    .ReturnsAsync(Convert.ToBoolean(expectedResult));

                //Act
                bool? actualResult = await _serviceUnderTest.DeleteEmployee(employeeId);

                //Assert
                Assert.Equal(expectedResult, actualResult);
            }
        }

        public class GetEmployeeById : EmployeeServiceTest
        {
            [Theory]
            [InlineData(1, 1)]            
            [InlineData(100, null)]
            public async Task Should_Return_Employee_Data(int employeeId, int? expectedResult)
            {
                //Assert
                _employeeDataAccessMock.Setup(x => x.GetEmployees()).ReturnsAsync(EmployeesDataMock.AsQueryable());

                //Act
                var actualResult = await _serviceUnderTest.GetEmployeeById(employeeId);

                //Assert
                if(expectedResult == null)
                {
                    Assert.Null(actualResult);
                }
                else
                {
                    Assert.Equal(employeeId, actualResult.Id);
                }
            }
        }

        public class GetEmployees : EmployeeServiceTest
        {
            [Fact]
            public async Task Should_Get_All_Employees()
            {
                //Arrange
                _employeeDataAccessMock.Setup(x => x.GetEmployees()).ReturnsAsync(EmployeesDataMock.AsQueryable());

                //Act
                var employees = await _serviceUnderTest.GetEmployees();

                //Assert
                Assert.True(employees.Any());
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
