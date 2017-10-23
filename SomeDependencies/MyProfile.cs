using AutoMapper;

namespace SomeDependencies
{
    public class MyProfile : Profile
    {
        public MyProfile(IConvertor convertor)
        {
            CreateMap<Model, ViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => convertor.Execute(src.SomeText)))
                ;

        }
    }
}
