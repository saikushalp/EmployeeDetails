using Abb.EmployeeDetails.Api.Models;
using Abb.EmployeeDetails.Api.Models.ViewModels;
using AutoMapper;

namespace Abb.EmployeeDetails.Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeCreateViewModel, Employees>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForMember(x => x.City, opt => opt.MapFrom(x => x.City))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
