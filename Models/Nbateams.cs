using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballWebApp.Models
{
    public partial class Nbateams
    {
        public Nbateams()
        {
            Players = new HashSet<Players>();
        }

        public int NbateamId { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public string Conference { get; set; }

        public virtual ICollection<Players> Players { get; set; }
    }
}
