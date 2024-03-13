using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.WebApi.Models
{

    public class UpdateNoteDto : IMapWith<UpdateNoteCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNoteDto, UpdateNoteCommand>()
                .ForMember(dst => dst.Title,
                 opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Details,
                 opt => opt.MapFrom(src => src.Details))
                .ForMember(dst => dst.Id,
                 opt => opt.MapFrom(src => src.Id));
        }
    }
}
