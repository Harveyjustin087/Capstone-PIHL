using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Player
    {
        public Player()
        {
            GoalRecordFirstAssistPlayers = new HashSet<GoalRecord>();
            GoalRecordScoringPlayers = new HashSet<GoalRecord>();
            GoalRecordSecondAssistPlayers = new HashSet<GoalRecord>();
            PenaltyRecords = new HashSet<PenaltyRecord>();
        }

        public int PlayerId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }
        [DisplayName("Goals")]
        public int? ScoreTotal { get; set; }
        [DisplayName("Assists")]
        public int? AssistTotal { get; set; }
        [DisplayName("Points")]
        public int? PointTotal { get; set; }
        [DisplayName("PIMs")]
        public TimeSpan? Pimtotal { get; set; }
        public int TeamId { get; set; }
        [DisplayName("Jersey Number")]
        public int JerseyNumber { get; set; }
        [NotMapped]
        public string NameandNumber
        {
            get { return Team.Name + " - " + JerseyNumber; }
        }

        public virtual Team Team { get; set; }
        public virtual ICollection<GoalRecord> GoalRecordFirstAssistPlayers { get; set; }
        public virtual ICollection<GoalRecord> GoalRecordScoringPlayers { get; set; }
        public virtual ICollection<GoalRecord> GoalRecordSecondAssistPlayers { get; set; }
        public virtual ICollection<PenaltyRecord> PenaltyRecords { get; set; }
    }
}
