using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace PIHLSite.Models
{
    public partial class GoalRecord
    {
        public int GoalRecordId { get; set; }
        [DisplayName("Game")]
        public int GameId { get; set; }
        [DisplayName("Scoring Player")]
        public int ScoringPlayerId { get; set; }
        [DisplayName("First Assist")]
        public int? FirstAssistPlayerId { get; set; }
        [DisplayName("Second Assist")]
        public int? SecondAssistPlayerId { get; set; }
        public int Period { get; set; }
        [DisplayName("Time of Goal")]
        public TimeSpan GameTime { get; set; }
        [DisplayName("First Assist")]
        public virtual Player FirstAssistPlayer { get; set; }
        public virtual Game Game { get; set; }
        [DisplayName("Goal Scorer")]
        public virtual Player ScoringPlayer { get; set; }
        [DisplayName("Second Assist")]
        public virtual Player SecondAssistPlayer { get; set; }
    }
}
