using FrontEnd.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FrontEnd.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                db.Database.EnsureCreated();
            }
        }
    }
}
