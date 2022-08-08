using AutoMapper;
using Cleverbit.Case.Models.Entities;
using Cleverbit.Case.Models.Requests;

namespace Cleverbit.Case.Business.MappingProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CreateRegionCommand, Region>();
            CreateMap<CreateEmployeeCommand, Employee>();
        }
    }
}
