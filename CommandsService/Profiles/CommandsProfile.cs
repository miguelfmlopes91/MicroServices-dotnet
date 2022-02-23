using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //source - > target (what we pass back)
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
        }
    }
}