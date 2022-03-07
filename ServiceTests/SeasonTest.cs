using Microsoft.EntityFrameworkCore;
using PIHLSite.Controllers;
using PIHLSite.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    [TestCaseOrderer("ServiceTests.PriorityOrderer","ServiceTests")]
    public class SeasonTest : IClassFixture<DbSetup>
    {

        Season season;
        DbSetup setup;

        public SeasonTest(DbSetup setup)
        {
            this.setup = setup;
        }


        [Fact, TestPriority(1)]
        public async Task SeasonController_CreateSeason_EnsureAddedAsync()
        {
            //Arrange
            season = new Season
            {
                StartYear = DateTime.Now,
                EndYear = DateTime.Now.AddDays(10)
            };
            SeasonController controller = new SeasonController(setup.PIHLDBContext);
            //Act
            var result = await controller.Create(season);
            //Assert
            var seasons = setup.PIHLDBContext.Seasons.ToListAsync();
            Assert.Single(seasons.Result);
        }

        [Fact, TestPriority(2)]
        public async Task SeasonController_EditSeason_EnsureEditedAsync()
        {
            //Arrange
            SeasonController controller = new SeasonController(setup.PIHLDBContext);
            season = await setup.PIHLDBContext.Seasons.FirstOrDefaultAsync();
            var checkDate = DateTime.Now.AddDays(30).ToShortDateString();

            //Act
            season.EndYear = DateTime.Now.AddDays(30);
            var result = await controller.Edit(season.SeasonId,season);
            //Assert
            season = await setup.PIHLDBContext.Seasons
                .FirstOrDefaultAsync(m => m.SeasonId == season.SeasonId);
            
            var seasonDate = season.EndYear.Value.ToShortDateString();
            Assert.Equal(checkDate, seasonDate);
        }

        [Fact, TestPriority(3)]
        public async Task SeasonController_DeleteSeason_EnsureDeletedAsync()
        {
            //Arrange
            SeasonController controller = new SeasonController(setup.PIHLDBContext);
            season = await setup.PIHLDBContext.Seasons.FirstOrDefaultAsync();

            //Act
            var result = await controller.DeleteConfirmed(season.SeasonId);
            //Assert
            var seasonList = await setup.PIHLDBContext.Seasons.ToListAsync();
            Assert.Empty(seasonList);
        }

    }
}
