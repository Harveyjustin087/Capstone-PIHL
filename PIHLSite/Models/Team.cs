using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Company { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Otl { get; set; }
        public int SeasonId { get; set; }

        public virtual Season Season { get; set; }
        public virtual ICollection<Game> GameAwayTeams { get; set; }
        public virtual ICollection<Game> GameHomeTeams { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
