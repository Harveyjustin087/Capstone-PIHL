using System;
using System.Collections.Generic;

#nullable disable

namespace PIHLSite.Models
{
    public partial class PenaltyRecord
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int PenaltyId { get; set; }
        public TimeSpan? Pim { get; set; }
        public int PenaltyRecordId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Penalty Penalty { get; set; }
        public virtual Player Player { get; set; }
    }
}
