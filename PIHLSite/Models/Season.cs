using System;
using System.Collections.Generic;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Season
    {
        public Season()
        {
            Teams = new HashSet<Team>();
        }
        [System.ComponentModel.DisplayName("Season Number")]
        public int SeasonId { get; set; }
        public DateTime? StartYear { get; set; }
        public DateTime? EndYear { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
