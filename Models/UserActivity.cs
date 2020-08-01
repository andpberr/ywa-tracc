using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ywa_tracc.Models
{
    public class UserActivity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string VideoId { get; set; }

        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }

        public string UserId { get; set; }
        public DateTime WatchDate { get; set; }
        public IdentityUser User { get; set; }
    }
}
