﻿using System;
using System.Collections.Generic;

namespace BasketballWebApp.Models
{
    public partial class UserTeams
    {
        public UserTeams()
        {
            UserTeamPlayers = new HashSet<UserTeamPlayers>();
        }

        public int UserTeamId { get; set; }
        public int UserId { get; set; }
        public decimal Budget { get; set; }
        public string TeamName { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<UserTeamPlayers> UserTeamPlayers { get; set; }
    }
}
