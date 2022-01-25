using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIHLSite.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementBody { get; set; }
        public string Author { get; set; }
        public DateTime AnnouncementDate { get; set; }
    }
}
