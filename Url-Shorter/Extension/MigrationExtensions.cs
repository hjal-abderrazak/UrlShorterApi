using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Url_Shorter.Extension
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations( this WebApplication app )
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
