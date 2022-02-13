using System;
using System.Collections.Generic;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Game
    {
        public Game()
        {
            GameTeams = new HashSet<GameTeam>();
            GoalRecords = new HashSet<GoalRecord>();
            PenaltyRecords = new HashSet<PenaltyRecord>();
        }

        public int GameId { get; set; }
        public DateTime GameDate { get; set; }
        public int HomeScoreTotal { get; set; }
        public int OppScoreTotal { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamId { get; set; }

        public virtual ICollection<GameTeam> GameTeams { get; set; }
        public virtual ICollection<GoalRecord> GoalRecords { get; set; }
        public virtual ICollection<PenaltyRecord> PenaltyRecords { get; set; }
    }
}
