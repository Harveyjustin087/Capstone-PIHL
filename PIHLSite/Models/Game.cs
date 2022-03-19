using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Game
    {
        public Game()
        {
            GoalRecords = new HashSet<GoalRecord>();
            PenaltyRecords = new HashSet<PenaltyRecord>();
        }

        public int GameId { get; set; }
        [DisplayName("Game Date")]
        public DateTime GameDate { get; set; }
        [DisplayName("Home Team Score")]
        public int HomeScoreTotal { get; set; }
        [DisplayName("Away Team Score")]
        public int AwayScoreTotal { get; set; }
        [DisplayName("Away Team")]
        public int AwayTeamId { get; set; }
        [DisplayName("Home Team")]
        public int HomeTeamId { get; set; }
        public bool Finalized { get; set; }
        public bool Overtime { get; set; }

        public virtual Team AwayTeam { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual ICollection<GoalRecord> GoalRecords { get; set; }
        public virtual ICollection<PenaltyRecord> PenaltyRecords { get; set; }
    }
}