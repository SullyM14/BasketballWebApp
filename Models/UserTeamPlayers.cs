using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballWebApp.Models
{
    public partial class UserTeamPlayers
    {


        [Display(Name = "Team ID")]
        public int UserTeamId { get; set; }

        [Display(Name = "Player ID")]
        public int PlayerId { get; set; }
        public int Id { get; set; }

        public virtual Players Player { get; set; }
        public virtual UserTeams UserTeam { get; set; }
    }
}
