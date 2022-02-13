using System;
using System.Linq;
using System.Security.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    /// <summary>
    /// A test class that creates db context
    /// </summary>
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data");
                
                context.Platforms.AddRange(
                    new Platform(){Name="Dot Net", Publisher="Microsoft", Cost = "Free"},
                    new Platform(){Name="Sql Server Express", Publisher="Microsoft", Cost = "Free"},
                    new Platform(){Name="Kubernetes", Publisher="Cloud Native", Cost = "Free"}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data");
            }
        }
    }
}