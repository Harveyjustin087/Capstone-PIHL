using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace PIHLSite.Models
{
    public partial class Team
    {
        public Team()
        {
            GameAwayTeams = new HashSet<Game>();
            GameHomeTeams = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        public int TeamId { get; set; }
        [DisplayName("Team Name")]
        public string Name { get; set; }
        public string Company { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        [DisplayName("OTL(Overtime Loss)")]
        public int Otl { get; set; }
        [DisplayName("Season")]
        public int SeasonId { get; set; }
        public string IDandName { get { return TeamId + " " + Name; } }

        public virtual Season Season { get; set; }
        public virtual ICollection<Game> GameAwayTeams { get; set; }
        public virtual ICollection<Game> GameHomeTeams { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
