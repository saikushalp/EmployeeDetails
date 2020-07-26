using Abb.EmployeeDetails.Api.DataAccess;
using Abb.EmployeeDetails.Api.DataAccess.Context;
using Abb.EmployeeDetails.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Abb.EmployeeDetails.Tests.DataAccess
{
    public class EmployeeDataAccessTest
    {
        private readonly ApplicationContext _applicationContextMock;
        private readonly EmployeeDataAccess _dataAccessUnderTest;

        public EmployeeDataAccessTest()
        {
            _applicationContextMock = GetInMemoryDatabase();
            _dataAccessUnderTest = new EmployeeDataAccess(_applicationContextMock);
        }

        public class GetEmployees : EmployeeDataAccessTest
        {
            [Fact]
            public async Task Should_Get_Employee_Details()
            {
                //Act
                var employees = await _dataAccessUnderTest.GetEmployees();

                //Assert
                Assert.True(employees.Any());
            }
        }

        public class CreateEmployee : EmployeeDataAccessTest
        {
            [Theory]
            [InlineData(5)]
            [InlineData(15)]
            [InlineData(100)]
            public async Task Should_Return_New_EmployeeId(int expectedEmployeeId)
            {
                //Arrange
                var employeeCreate = EmployeesDataMock.FirstOrDefault();
                employeeCreate.Id = expectedEmployeeId;
                //Act
                int actualEmployeeId = await _dataAccessUnderTest.CreateEmployee(employeeCreate);

                //Assert
                Assert.Equal(expectedEmployeeId, actualEmployeeId);
            }
        }

        public class DeleteEmployee : EmployeeDataAccessTest
        {
            [Theory]
            [InlineData(1, true)]
            [InlineData(100, false)]
            public async Task Should_Delete_Employee(int employeeId, bool expectedResult)
            {
                //Act
                bool actualResult = await _dataAccessUnderTest.DeleteEmployee(employeeId);

                //Assert
                Assert.Equal(expectedResult, actualResult);
            }
        }
        
        private ApplicationContext GetInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging()
                      .Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(EmployeesDataMock);           
            context.SaveChanges();

            return context;
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
