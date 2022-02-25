using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();
        
        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatorm(Platform platform);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExist(int externalPlatformId);
        
        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformID);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}