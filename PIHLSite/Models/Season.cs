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

        public int SeasonId { get; set; }
        [DisplayName("Begin Date")]
        public DateTime? StartYear { get; set; }
        [DisplayName("End Date")]
        public DateTime? EndYear { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
