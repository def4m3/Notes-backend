﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebApi.Models
{
    public class CreateNoteDto : IMapWith<CreateNoteCommand>
    {
        [Required]
        public string Title { get; set; }
        public string Details { get; set; }

        public void Mapping (Profile profile)
        {
            profile.CreateMap<CreateNoteDto,CreateNoteCommand>()
                .ForMember(dst => dst.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Details, 
                opt => opt.MapFrom(src => src.Details));
        }
    }
}
