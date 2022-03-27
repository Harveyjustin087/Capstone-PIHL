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
    public class TeamTests : IClassFixture<DbSetup>
    {
        DbSetup setup;
        Team team;
        Season season;
        public TeamTests(DbSetup setup)
        {
            this.setup = setup;
        }

        [Fact, TestPriority(1)]
        public async Task TeamController_AddTeam_EnsureTeamAdded()
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
            //Act
            var result = await tController.Create(team);
            //Assert
            var teams = setup.PIHLDBContext.Teams.ToListAsync();
            Assert.Single(teams.Result);
        }

        [Fact, TestPriority(2)]
        public async Task TeamController_EditTeam_EnsureEditedAsync()
        {
            //Arrange
            TeamController controller = new TeamController(setup.PIHLDBContext);
            team = await setup.PIHLDBContext.Teams.FirstOrDefaultAsync();
            //Act
            team.Win = 1;
            team.Loss = 3;
            var result = await controller.Edit(team.TeamId, team);
            //Assert
            team = await setup.PIHLDBContext.Teams
                .FirstOrDefaultAsync(m => m.TeamId == team.TeamId);
            Assert.Equal(1, team.Win);
            Assert.Equal(3, team.Loss);
        }

        [Fact, TestPriority(3)]
        public async Task TeamController_DeleteTeam_EnsureDeletedAsync()
        {
            //Arrange
            TeamController controller = new TeamController(setup.PIHLDBContext);
            team = await setup.PIHLDBContext.Teams.FirstOrDefaultAsync();
            //Act
            var result = await controller.DeleteConfirmed(team.TeamId);
            //Assert
            var teamList = await setup.PIHLDBContext.Teams.ToListAsync();
            Assert.Empty(teamList);
        }

    }
}
