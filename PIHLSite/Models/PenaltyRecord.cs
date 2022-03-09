using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace PIHLSite.Models
{
    public partial class PenaltyRecord
    {
        public int PenaltyRecordId { get; set; }
        [DisplayName("Game Number")]
        public int GameId { get; set; }
        [DisplayName("Player")]
        public int PlayerId { get; set; }
        [DisplayName("Penalty Type")]
        public int PenaltyId { get; set; }
        [DisplayName("PIM(Penalty in Minutes)")]
        public TimeSpan? Pim { get; set; }

        public virtual Game Game { get; set; }
        public virtual Penalty Penalty { get; set; }
        public virtual Player Player { get; set; }
    }
}
