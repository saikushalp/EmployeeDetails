using Abb.EmployeeDetails.Api.Models.ViewModels;
using FluentValidation;

namespace Abb.EmployeeDetails.Api.Validator
{
    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateViewModel>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName_Cannot_Be_Null_or_Empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName_Cannot_Be_Null_or_Empty");
            RuleFor(x => x.City).NotEmpty().WithMessage("City_Cannot_Be_Null_or_Empty");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("DateOfBith_Is_Invalid");
        }
    }
}
