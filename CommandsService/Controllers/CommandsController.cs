using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform {platformId}");
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandID}", Name = "GetCommandForPlatform")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandForPlatform(int platformId, int commandID)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform {platformId} / {commandID}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = _repository.GetCommand(platformId, commandID);

            if (command == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(command));
        }
        
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }            
            
            //create table that holds the command model
            var commmand = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(platformId, commmand);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commmand);
            
            return CreatedAtRoute(
                nameof(GetCommandForPlatform),
                new {platformId = platformId, commandId = commandReadDto.Id},
                commandReadDto
                );
        }
    }
}