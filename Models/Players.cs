using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballWebApp.Models
{
    public partial class Players
    {
        public Players()
        {
            UserTeamPlayers = new HashSet<UserTeamPlayers>();
        }

        public int PlayerId { get; set; }

        [Display(Name = "First Name")]

        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }

        [Display(Name = "PPG")]
        public decimal Ppg { get; set; }
        [Display(Name = "APG")]
        public decimal Apg { get; set; }
        [Display(Name = "RPG")]
        public decimal Rpg { get; set; }
        public int? Points { get; set; }
        public int? Rebounds { get; set; }
        public int? Assists { get; set; }
        public int TeamId { get; set; }
        [Display(Name = "Games Played")]
        public int? GamesPlayed { get; set; }
        public string Position { get; set; }
        public decimal Price { get; set; }

        public virtual Nbateams Team { get; set; }
        public virtual ICollection<UserTeamPlayers> UserTeamPlayers { get; set; }
    }
}
