using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ywa_tracc.Models;

namespace ywa_tracc.ViewModels
{
    public class UserActivityViewModel
    {
        public string VideoID { get; set; }
        public Video video { get; set; }
    }
}
