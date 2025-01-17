﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketballWebApp.Models;

namespace BasketballWebApp.Service
{
    public interface ICRUD
    {
        IOrderedQueryable<Nbateams> RetrieveNbaTeams();
        IOrderedQueryable<Players> RetrieveTeamPlayers(int? id);
        Task<Players> RetrievePlayerDetails(int? id);
        IQueryable<UserTeams> AllUserTeams();
        IQueryable<UserTeamPlayers> RetrieveUserTeamsPlayers(int? id);
        Task<UserTeams> RetrieveUserTeamsDetails(int? id);

        Task RemoveUserTeam(int? id);
        Task CreateNewTeam(UserTeams userTeam);
        Task AddPlayer(UserTeamPlayers userTeamPlayers);
        string RetrieveUserName(int id);
        IEnumerable<Players> RetrievePlayers();
        IEnumerable<UserTeams> RetrieveUserTeam(int? id);
        Task<UserTeamPlayers> RetrieveUserPlayer(int? id);

        Task RemoveUserPlayer(int? id);



    }
}
