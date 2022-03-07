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
    public class ScorekeeperTests : IClassFixture<DbSetup>
    {
        DbSetup setup;
        Team homeTeam;
        Team awayTeam;
        Season season;
        Game game;

        public ScorekeeperTests(DbSetup setup)
        {
            this.setup = setup;
        }

        [Fact, TestPriority(1)]
        public async Task ScorekeeperController_CreateGame_EnsureCreated()
        {
            //Arrange
            season = new Season
            {
                StartYear = DateTime.Now,
                EndYear = DateTime.Now.AddDays(10)
            };
            SeasonController sController = new SeasonController(setup.PIHLDBContext);
            await sController.Create(season);
            homeTeam = new Team
            {
                Name = "Maple Leafs",
                Win = 0,
                Loss = 0,
                Otl = 0,
                SeasonId = 1
            };
            awayTeam = new Team
            {
                Name = "Golden Knights",
                Win = 0,
                Loss = 0,
                Otl = 0,
                SeasonId = 1
            };
            TeamController tController = new TeamController(setup.PIHLDBContext);
            await tController.Create(homeTeam);
            await tController.Create(awayTeam);
            awayTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Golden Knights").FirstOrDefault();
            homeTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Maple Leafs").FirstOrDefault();
            game = new Game
            {
                AwayTeamId = awayTeam.TeamId,
                HomeTeamId = homeTeam.TeamId,
                Finalized = false,
                Overtime = false,
                GameDate = DateTime.Now
            };

            //Act
            ScorekeeperController controller = new ScorekeeperController(setup.PIHLDBContext);
            await controller.Create(game);

            //Assert
            var games = setup.PIHLDBContext.Games.ToListAsync();
            Assert.Single(games.Result);
        }

        [Fact, TestPriority(2)]
        public async Task ScorekeeperController_EditGame_EnsureEdited()
        {
            //Arrange
            game = setup.PIHLDBContext.Games.FirstOrDefault();
            ScorekeeperController controller = new ScorekeeperController(setup.PIHLDBContext);
            //Act
            game.AwayScoreTotal = 2;
            await controller.Edit(game.GameId, game);
            game = setup.PIHLDBContext.Games.FirstOrDefault();
            //Assert
            Assert.Equal(2, game.AwayScoreTotal);

        }

        [Fact, TestPriority(3)]
        public async Task ScorekeeperController_FinalizeGame_EnsureFinalized()
        {
            //Arrange
            game = setup.PIHLDBContext.Games.FirstOrDefault();
            ScorekeeperController controller = new ScorekeeperController(setup.PIHLDBContext);
            //Act
            await controller.Finalize(game.GameId, game);
            game = setup.PIHLDBContext.Games.FirstOrDefault();
            awayTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Golden Knights").FirstOrDefault();
            homeTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Maple Leafs").FirstOrDefault();
            //Assert
            Assert.True(game.Finalized);
            Assert.Equal(1, awayTeam.Win);
            Assert.Equal(1, homeTeam.Loss);
        }

        [Fact, TestPriority(4)]
        public async Task ScorekeeperController_DeleteGame_EnsureDeleted()
        {
            //Arrange
            game = setup.PIHLDBContext.Games.FirstOrDefault();
            ScorekeeperController controller = new ScorekeeperController(setup.PIHLDBContext);
            //Act
            await controller.DeleteConfirmed(game.GameId);
            var games = setup.PIHLDBContext.Games.ToListAsync();
            awayTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Golden Knights").FirstOrDefault();
            homeTeam = setup.PIHLDBContext.Teams.Where(n => n.Name == "Maple Leafs").FirstOrDefault();
            //Assert
            Assert.Empty(games.Result);
            Assert.Equal(0, awayTeam.Win);
            Assert.Equal(0, awayTeam.Loss);
        }

    }
}
