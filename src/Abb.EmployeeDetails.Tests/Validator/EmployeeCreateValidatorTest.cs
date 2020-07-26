using Abb.EmployeeDetails.Api.Models.ViewModels;
using Abb.EmployeeDetails.Api.Validator;
using System;
using System.Linq;
using Xunit;

namespace Abb.EmployeeDetails.Tests.Validator
{
    public class EmployeeCreateValidatorTest
    {
        private readonly EmployeeCreateValidator validatorUnderTest;

        public EmployeeCreateValidatorTest()
        {
            validatorUnderTest = new EmployeeCreateValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_FirstName_When_Null_Or_Empty(string firstName)
        {
            //Arrange
            string expectedResult = "FirstName_Cannot_Be_Null_or_Empty";
            var createEmployee = employeeCreateDataMock;
            createEmployee.FirstName = firstName;

            //Act
            var actualResult = validatorUnderTest.Validate(createEmployee);

            //Assert
            Assert.Equal(expectedResult, actualResult.Errors[0].ErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_LastName_When_Null_Or_Empty(string lastName)
        {
            //Arrange
            string expectedResult = "LastName_Cannot_Be_Null_or_Empty";
            var createEmployee = employeeCreateDataMock;
            createEmployee.LastName = lastName;

            //Act
            var actualResult = validatorUnderTest.Validate(createEmployee);

            //Assert
            Assert.Equal(expectedResult, actualResult.Errors[0].ErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_City_When_Null_Or_Empty(string city)
        {
            //Arrange
            string expectedResult = "City_Cannot_Be_Null_or_Empty";
            var createEmployee = employeeCreateDataMock;
            createEmployee.City = city;

            //Act
            var actualResult = validatorUnderTest.Validate(createEmployee);

            //Assert
            Assert.Equal(expectedResult, actualResult.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Should_validate_Date_When_It_Is_Default()
        {
            //Arrange
            string expectedResult = "DateOfBith_Is_Invalid";
            var createEmployee = employeeCreateDataMock;
            createEmployee.DateOfBirth = DateTime.MinValue;

            //Act
            var actualResult = validatorUnderTest.Validate(createEmployee);

            //Assert
            Assert.Equal(expectedResult, actualResult.Errors[0].ErrorMessage);
        }


        [Fact]
        public void Should_validate_EmployeeCreate_With_All_Valid_Values()
        {
            //Arrange            
            var createEmployee = employeeCreateDataMock;

            //Act
            var actualResult = validatorUnderTest.Validate(createEmployee);

            //Assert
            Assert.False(actualResult.Errors.Any());
        }

        private static EmployeeCreateViewModel employeeCreateDataMock => new EmployeeCreateViewModel
        {
            FirstName = "John",
            LastName = "Smith",
            City = "Newyork",
            DateOfBirth = DateTime.Now.AddYears(-20)
        };
    }
}
