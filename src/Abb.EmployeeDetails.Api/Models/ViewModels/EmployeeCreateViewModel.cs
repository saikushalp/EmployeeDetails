using System;

namespace Abb.EmployeeDetails.Api.Models.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
    }
}
