using System;
using System.Collections.Generic;

#nullable disable

namespace PIHLSite.Models
{
    public partial class GoalRecord
    {
        public int GoalRecordId { get; set; }
        public int GameId { get; set; }
        public int ScoringPlayerId { get; set; }
        public int? FirstAssistPlayerId { get; set; }
        public int? SecondAssistPlayerId { get; set; }
        public int Period { get; set; }
        public TimeSpan GameTime { get; set; }

        public virtual Player FirstAssistPlayer { get; set; }
        public virtual Game Game { get; set; }
        public virtual Player ScoringPlayer { get; set; }
        public virtual Player SecondAssistPlayer { get; set; }
    }
}
