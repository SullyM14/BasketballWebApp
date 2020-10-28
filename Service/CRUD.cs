using BasketballWebApp.Models;
using BasketballWebApp.Service;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketballBusinessLayer
{
    public class CRUD : ICRUD
    {

        private BasketballProjectContext _context;

        public CRUD(BasketballProjectContext context)
        {
            _context = context;
        }

        public IOrderedQueryable<Nbateams> RetrieveNbaTeams()
        {
            var teams = _context.Nbateams.OrderByDescending(t => t.Wins);
            return teams;
        }

        public IOrderedQueryable<Players> RetrieveTeamPlayers(int? id)
        {
            var players = _context.Players.Where(p => p.TeamId == id).OrderByDescending(p => p.Price);
            return players;
        }

        public async Task<Players> RetrievePlayerDetails(int? id)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == id);
            return player;
        }


        public IEnumerable<Players> RetrievePlayers()
        {
            return _context.Players.ToList();
        }

        public IEnumerable<UserTeams> RetrieveUserTeam(int? id)
        {
            return _context.UserTeams.Where(ut => ut.UserTeamId == id).ToList();
        }

        public async Task<UserTeamPlayers> RetrieveUserPlayer(int? id)
        {
            var userTeamPlayers = await _context.UserTeamPlayers
                                    .Include(u => u.Player)
                                    .Include(u => u.UserTeam)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            return userTeamPlayers;
        }


        public IQueryable<UserTeams> AllUserTeams()
        {
            var fantasyTeam = _context.UserTeams.Include(u => u.User).Where(ut => ut.UserId == 1);
            return fantasyTeam;
        }

        public IQueryable<UserTeamPlayers> RetrieveUserTeamsPlayers(int? id)
        {
            var fantasyPlayers = from uTeamPlayers in _context.UserTeamPlayers.Include(ut => ut.UserTeam).Include(p => p.Player)
                                 where (uTeamPlayers.UserTeamId == id)
                                 select uTeamPlayers;
            return fantasyPlayers;
        }

        public async Task<UserTeams> RetrieveUserTeamsDetails(int? id)
        {
            var teamDetails = await _context.UserTeams.FirstOrDefaultAsync(ut => ut.UserTeamId == id);
            return teamDetails;
        }

        public async Task RemoveUserTeam(int? id)
        {
            var userTeamPlayers = _context.UserTeamPlayers.Where(ut => ut.UserTeamId == id).ToList();
            if (userTeamPlayers != null)
            {
                _context.RemoveRange(userTeamPlayers);
                await _context.SaveChangesAsync();
            }
            var team = _context.UserTeams.Where(ut => ut.UserTeamId == id).FirstOrDefault();
            _context.UserTeams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewTeam(UserTeams userTeam)
        {
            _context.Add(userTeam);
            await _context.SaveChangesAsync();
        }

        public async Task AddPlayer(UserTeamPlayers userTeamPlayers)
        {
            var player = _context.Players.Where(p => p.PlayerId == userTeamPlayers.PlayerId).FirstOrDefault();
            var team = _context.UserTeams.Where(u => u.UserTeamId == userTeamPlayers.UserTeamId).FirstOrDefault();

            var numberOfPlayersInTeam = RetrieveUserTeamsPlayers(userTeamPlayers.UserTeamId).Count(); 
            var isPlayerAlreadyInTeam = _context.UserTeamPlayers.Where(ut => ut.UserTeamId == userTeamPlayers.UserTeamId).Where(ut => ut.PlayerId == userTeamPlayers.PlayerId).Count();

            if (isPlayerAlreadyInTeam != 1)
            {
                if (numberOfPlayersInTeam < 6)
                    if (CheckBudget(team.Budget, player.Price) == true)
                    {
                        //Update budget and then add the player
                        team.Budget -= player.Price;
                        _context.UserTeams.Where(ut => ut.UserTeamId == team.UserTeamId);
                        await _context.SaveChangesAsync();

                        var newTeam = new UserTeamPlayers { PlayerId = player.PlayerId, UserTeamId = team.UserTeamId };
                        _context.UserTeamPlayers.Add(newTeam);
                        await _context.SaveChangesAsync();
                    }
                  else{throw new OutOfBudgetException();}
                else{throw new TooManyPlayerException();}
            }
        }

        public bool CheckBudget(decimal budget, decimal price)
        {
            return price <= budget;
        }

        public async Task RemoveUserPlayer(int? id)
        {

            var userTeamPlayers = await _context.UserTeamPlayers.FindAsync(id);
            var player = _context.Players.Where(p => p.PlayerId == userTeamPlayers.PlayerId).FirstOrDefault();
            var team = _context.UserTeams.Where(u => u.UserTeamId == userTeamPlayers.UserTeamId).FirstOrDefault();
            team.Budget += player.Price;
            _context.UserTeams.Update(team);
            await _context.SaveChangesAsync();
            _context.UserTeamPlayers.Remove(userTeamPlayers);
            await _context.SaveChangesAsync();
        }

        public string RetrieveUserName(int id)
        {
            return _context.Users.Where(u => u.UserId == 1).FirstOrDefault().FirstName;
        }
    }
}