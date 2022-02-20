using System;
using System.Linq;
using System.Security.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    /// <summary>
    /// A test class that creates db context
    /// </summary>
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("---> Attemping to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not run migrations {e.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding data");
                
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