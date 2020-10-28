using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballWebApp.Models
{
    public partial class UserTeams
    {
        public UserTeams()
        {
            UserTeamPlayers = new HashSet<UserTeamPlayers>();
        }

        [Display(Name = "Team ID")]
        public int UserTeamId { get; set; }


        [Display(Name = "User ID")]
        public int UserId { get; set; }
        public decimal Budget { get; set; } = 100;


        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<UserTeamPlayers> UserTeamPlayers { get; set; }
    }
}
