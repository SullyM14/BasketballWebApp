using System;
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
    }
}
