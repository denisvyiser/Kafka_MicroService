using AutoMapper;
using BBari.Application.Commands;
using BBari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Application.AutoMapper
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, NewMessage>().ConstructUsing(c => new NewMessage(c.Id, c.ServiceId, c.Data, c.TimeStamp)).ReverseMap();

            //CreateMap<Message, NewMessage>()
            //.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            //.ForMember(dest => dest.ServiceId, opts => opts.MapFrom(src => src.ServiceId))
            //.ForMember(dest => dest.Data, opts => opts.MapFrom(src => src.Data))
            //.ForMember(dest => dest.TimeStamp, opts => opts.MapFrom(src => src.TimeStamp));
        }
    }
}
