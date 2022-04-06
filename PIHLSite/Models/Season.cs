using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("Season Start")]
        public DateTime? StartYear { get; set; }
        [DisplayName("Season End")]
        public DateTime? EndYear { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
