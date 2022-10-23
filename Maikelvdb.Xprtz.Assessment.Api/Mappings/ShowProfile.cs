using Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands;

namespace Maikelvdb.Xprtz.Assessment.Api.Mappings
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            CreateMap<Show, ShowDto>();

            CreateMap<CreateShowCommand, Show>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.CreatedDate, m => m.Ignore())
                .ForMember(d => d.ModifiedDate, m => m.Ignore())
                .ForMember(d => d.ExternalId, m => m.Ignore());

            CreateMap<UpdateShowCommand, Show>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.CreatedDate, m => m.Ignore())
                .ForMember(d => d.ModifiedDate, m => m.Ignore());
        }
    }
}
