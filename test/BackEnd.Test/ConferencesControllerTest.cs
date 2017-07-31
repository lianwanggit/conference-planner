using BackEnd.Controllers;
using BackEnd.Data;
using ConferenceDTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BackEnd.Test
{
    public class ConferencesControllerTest
    {
        [Fact]
        public async Task Get_conferences()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Get_conferences")
                .Options;

            await GetTestConferences(options);

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ConferencesController(context);
                var result = await controller.GetConferences();

                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<List<ConferenceResponse>>(okResult.Value);

                Assert.Equal(2, response.Count);
                Assert.Contains(response, c => c.Name == "Conference1");
                Assert.Contains(response, c => c.Name == "Conference2");
            }
        }

        [Fact]
        public async Task Get_conference_by_id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Get_conferences")
                .Options;

            await GetTestConferences(options);
            var id = 1;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ConferencesController(context);
                var result = await controller.GetConference(id);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<ConferenceResponse>(okResult.Value);

                Assert.Equal("Conference1", response.Name); 
            }
        }

        private async Task GetTestConferences(DbContextOptions<ApplicationDbContext> options)
        {
            using (var context = new ApplicationDbContext(options))
            {
                var conference1 = new Conference
                {
                    Name = "Conference1"
                };
                var conference2 = new Conference
                {
                    Name = "Conference2"
                };

                context.Conferences.Add(conference1);
                context.Conferences.Add(conference2);
                await context.SaveChangesAsync();
            }
        }
    }
}
