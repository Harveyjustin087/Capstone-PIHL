using Microsoft.EntityFrameworkCore;
using PIHLSite.Controllers;
using PIHLSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    [TestCaseOrderer("ServiceTests.PriorityOrderer", "ServiceTests")]
    public class PlayerTest : IClassFixture<DbSetup>
    {
        DbSetup setup;
        Team team;
        Player player;
        Season season;
        public PlayerTest(DbSetup setup)
        {
            this.setup = setup;
        }

        [Fact, TestPriority(1)]
        public async Task PlayerController_CreatePlayer_EnsurePlayerAdded()
        {
            //Arrange
            season = new Season
            {
                StartYear = DateTime.Now,
                EndYear = DateTime.Now.AddDays(10)
            };
            SeasonController sController = new SeasonController(setup.PIHLDBContext);
            await sController.Create(season);
            team = new Team
            {
                Name = "Maple Leafs",
                Win = 0,
                Loss = 0,
                Otl = 0,
                SeasonId = 1
            };
            TeamController tController = new TeamController(setup.PIHLDBContext);
            var result = await tController.Create(team);
            player = new Player
            {
                TeamId = 1,
                FirstName = "John",
                LastName = "Doe",
                Age = 25,
                AssistTotal = 0,
                Pimtotal = TimeSpan.Zero,
                PointTotal = 0,
                ScoreTotal = 0
            };
            //Act
            PlayerController controller = new PlayerController(setup.PIHLDBContext);
            await controller.Create(player);
            //Assert
            var teamData = setup.PIHLDBContext.Teams.Include(t => t.Players).ToListAsync();
            Assert.Single(teamData.Result[0].Players);
        }

        [Fact, TestPriority(2)]
        public async Task PlayerController_EditPlayer_EnsurePlayerEdited()
        {
            //Arrange
            player = setup.PIHLDBContext.Players.FirstOrDefault();
            PlayerController controller = new PlayerController(setup.PIHLDBContext);
            //Act
            player.Age = 29;
            await controller.Edit(player.PlayerId, player);
            //Assert
            player = setup.PIHLDBContext.Players.Where(p => p.PlayerId == player.PlayerId).FirstOrDefault();
            Assert.Equal(29, player.Age);
        }

        [Fact, TestPriority(3)]
        public async Task PlayerController_DeletePlayer_EnsurePlayerDeleted()
        {
            //Arrange
            player = setup.PIHLDBContext.Players.FirstOrDefault();
            PlayerController controller = new PlayerController(setup.PIHLDBContext);
            //Act
            await controller.DeleteConfirmed(player.PlayerId);
            //Assert
            var teamData = setup.PIHLDBContext.Teams.Include(t => t.Players).ToListAsync();
            Assert.Empty(teamData.Result[0].Players);
        }
    }
}
