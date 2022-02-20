using System;
using System.Collections.Generic;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Penalty
    {
        public Penalty()
        {
            PenaltyRecords = new HashSet<PenaltyRecord>();
        }

        public int PenaltyId { get; set; }
        public string PenaltyCode { get; set; }
        public string PenaltyDescription { get; set; }

        public virtual ICollection<PenaltyRecord> PenaltyRecords { get; set; }
    }
}
