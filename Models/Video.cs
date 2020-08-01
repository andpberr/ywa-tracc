using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ywa_tracc.Models
{
    public class Video
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ThumbnailURL { get; set; }
        public DateTime PostedDate { get; set; }
        public int Duration { get; set; }
    }
}
