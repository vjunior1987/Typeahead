using AutoMapper;
using Typeahead.DAL;

namespace Typeahead.Service
{
    public class NameProfile : Profile
    {
        public NameProfile()
        {
            CreateMap<Name, NameDTO>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Value))
                .ReverseMap()
                .ForMember(src => src.Value, options => options.MapFrom(dest => dest.Name));
        }
    }
}
